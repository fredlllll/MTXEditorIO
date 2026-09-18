using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Col
{
    // BSP tree encoding (reverse engineered, verified against MTX Mototrax PRO .col files):
    //   - every node is 8 bytes; some files start the node array with a 4-byte prefix
    //     holding the node-array size in bytes (e.g. 0x648 = 201 nodes * 8)
    //   - split node:  { u32 splitAxisAndPoint (low 2 bits = axis, upper 30 = int(16*splitPoint)), u32 leftNodeOffset }
    //     children of a split are the node at leftNodeOffset/8 and the node right after it (leftNodeOffset/8 + 1)
    //   - leaf node:   { u32 0xNNNN0003 (numFaces = high u16), u32 nodeFaceIndexOffset (offset in u16 units
    //     into the file-global face-index pool that follows the last object's node array) }
    public class ColBSPTree : IReadableWriteable
    {
        const int BSPNodeSize = 8;

        public bool hasPrefix;
        public int nodeCount;
        public ColBSPNode[] nodes = Array.Empty<ColBSPNode>();
        public TreeNode root = null!;

        // Resolve the node array (prefix + node count) at the start of `block`.
        // `numFaces` and `hasTrailingPool` let us reject candidates that swallow the
        // face-index pool (used for the last object in the file, whose block extends to EOF).
        public static void Resolve(byte[] block, int numFaces, bool hasTrailingPool, out bool prefix, out int count)
        {
            prefix = false;
            count = 0;

            // prefix candidate: first u32 = node-array size in bytes
            if (block.Length >= 4)
            {
                uint first = BitConverter.ToUInt32(block, 0);
                if (first % BSPNodeSize == 0)
                {
                    long n = first / BSPNodeSize;
                    if (n >= 1 && 4 + n * BSPNodeSize <= block.Length && StructurallyConsistent(block, 4, (int)n))
                    {
                        if (!hasTrailingPool || PoolFits(block, 4 + (int)n * BSPNodeSize, numFaces, CollectLeaves(block, 4, (int)n)))
                        {
                            prefix = true;
                            count = (int)n;
                            return;
                        }
                    }
                }
            }

            // no-prefix candidate: largest odd count that forms a consistent full binary tree
            int maxNodes = block.Length / BSPNodeSize;
            for (int n = (maxNodes % 2 == 0 ? maxNodes - 1 : maxNodes); n >= 1; n -= 2)
            {
                if (!StructurallyConsistent(block, 0, n)) continue;
                if (hasTrailingPool && !PoolFits(block, n * BSPNodeSize, numFaces, CollectLeaves(block, 0, n))) continue;
                prefix = false;
                count = n;
                return;
            }
        }

        // Parse the node array at `dataStart` into `nodes` and (re)build the tree.
        public void Populate(byte[] block, int dataStart, int count)
        {
            hasPrefix = dataStart == 4;
            nodeCount = count;

            nodes = new ColBSPNode[nodeCount];
            using (var ms = new MemoryStream(block, dataStart, nodeCount * BSPNodeSize, false))
            using (var br = new BinaryReader(ms, Encoding.ASCII, true))
            {
                for (int i = 0; i < nodeCount; ++i)
                {
                    var n = nodes[i] = new ColBSPNode();
                    n.ReadFrom(br);
                }
            }

            root = BuildTree();
        }

        public void ReadFrom(BinaryReader reader)
        {
            long start = reader.BaseStream.Position;
            long avail = reader.BaseStream.Length - start;
            if (avail <= 0)
            {
                hasPrefix = false;
                nodeCount = 0;
                nodes = Array.Empty<ColBSPNode>();
                root = new TreeNode { isLeaf = true };
                return;
            }

            var block = new byte[avail];
            reader.Read(block, 0, block.Length);

            // convenience path: treat the whole remaining stream as one block with a trailing pool
            Resolve(block, 0, true, out bool prefix, out int count);

            int dataStart = prefix ? 4 : 0;
            Populate(block, dataStart, count);
            reader.BaseStream.Position = start + dataStart + nodeCount * BSPNodeSize;
        }

        public void WriteTo(BinaryWriter writer)
        {
            if (hasPrefix)
            {
                writer.Write((uint)(nodeCount * BSPNodeSize));
            }
            for (int i = 0; i < nodeCount; ++i)
            {
                nodes[i].WriteTo(writer);
            }
        }

        private TreeNode BuildTree()
        {
            var t = new TreeNode[nodeCount];
            for (int i = 0; i < nodeCount; ++i)
            {
                t[i] = new TreeNode { nodeIndex = i };
            }

            for (int i = 0; i < nodeCount; ++i)
            {
                var bn = nodes[i];
                var tn = t[i];

                if (bn.type == BSPSplitAxis.Leaf)
                {
                    tn.isLeaf = true;
                    tn.leafData = new TreeNode.LeafData
                    {
                        numFaces = bn.leaf.numFaces,
                        nodeFaceIndexOffset = bn.leaf.nodeFaceIndexOffset
                    };
                }
                else
                {
                    long l = bn.axisSplit.leftNodeOffset / BSPNodeSize;
                    long r = l + 1;

                    tn.isLeaf = false;
                    tn.nodeData = new TreeNode.NodeData
                    {
                        splitAxis = bn.axisSplit.SplitAxis,
                        splitPoint = bn.axisSplit.SplitPoint,
                        leftNodeOffset = bn.axisSplit.leftNodeOffset
                    };

                    if (l >= 0 && r < nodeCount)
                    {
                        tn.nodeData.left = t[l];
                        tn.nodeData.right = t[r];
                    }
                    else
                    {
                        // malformed child index -> degrade to leaf so the read stays non-fatal
                        tn.isLeaf = true;
                        tn.leafData = new TreeNode.LeafData();
                    }
                }
            }

            return nodeCount > 0 ? t[0] : new TreeNode { isLeaf = true };
        }

        private static bool StructurallyConsistent(byte[] block, int start, int count)
        {
            if (count < 1 || count % 2 == 0) return false;
            if (start < 0 || count * BSPNodeSize > block.Length - start) return false;

            var refs = new int[count];
            for (int i = 0; i < count; ++i)
            {
                uint first = BitConverter.ToUInt32(block, start + i * BSPNodeSize);
                if ((first & 3u) == 3) continue; // leaf

                uint second = BitConverter.ToUInt32(block, start + i * BSPNodeSize + 4);
                long l = second / BSPNodeSize;
                long r = l + 1;
                if (l < 0 || r >= count) return false;
                if (++refs[l] > 1 || ++refs[r] > 1) return false;
            }

            if (refs[0] != 0) return false;
            int roots = 0;
            for (int i = 0; i < count; ++i)
            {
                if (refs[i] == 0) roots++;
            }
            return roots == 1;
        }

        private static List<(uint off, int faces)> CollectLeaves(byte[] block, int start, int count)
        {
            var leaves = new List<(uint, int)>();
            for (int i = 0; i < count; ++i)
            {
                uint a = BitConverter.ToUInt32(block, start + i * BSPNodeSize);
                if ((a & 3u) != 3) continue;
                uint off = BitConverter.ToUInt32(block, start + i * BSPNodeSize + 4);
                int faces = (int)((a >> 16) & 0xFFFF);
                leaves.Add((off, faces));
            }
            return leaves;
        }

        // Reject a candidate when the trailing region it leaves behind (from `consumed` to
        // end of block) cannot be the face-index pool: ranges must stay within the pool and
        // not overlap, and referenced face indices must be valid for this object.
        private static bool PoolFits(byte[] block, int consumed, int numFaces, List<(uint off, int faces)> leaves)
        {
            int poolBytes = block.Length - consumed;
            if (poolBytes < 0 || poolBytes % 2 != 0) return false;
            int poolU16 = poolBytes / 2;

            foreach (var (off, faces) in leaves)
            {
                if (off + (uint)faces > (uint)poolU16) return false;
            }

            // zero-face leaves (empty-object / sentinel leaves) may sit anywhere, including
            // at a boundary shared with a real leaf, so exclude them from the overlap check
            var ordered = new List<(uint off, int faces)>();
            foreach (var l in leaves)
            {
                if (l.faces > 0) ordered.Add(l);
            }
            ordered.Sort((x, y) => x.off.CompareTo(y.off));
            for (int i = 1; i < ordered.Count; ++i)
            {
                var prev = ordered[i - 1];
                if (prev.off + (uint)prev.faces > ordered[i].off) return false; // overlap
            }

            if (poolU16 == 0) return leaves.Count == 0 || leaves.TrueForAll(l => l.faces == 0);

            for (int i = 0; i < leaves.Count; ++i)
            {
                var (off, faces) = leaves[i];
                if (off + (uint)faces > (uint)poolU16) continue;
                for (int k = 0; k < faces; ++k)
                {
                    ushort fi = (ushort)(block[consumed + ((int)off + k) * 2] | (block[consumed + ((int)off + k) * 2 + 1] << 8));
                    if (fi >= numFaces) return false;
                }
            }

            return true;
        }
    }

    public class TreeNode
    {
        public class LeafData
        {
            public ushort numFaces;
            public uint nodeFaceIndexOffset; //offset of the face list inside the bsp faces block
        }

        public class NodeData
        {
            public TreeNode? left, right;
            public BSPSplitAxis splitAxis;
            public uint splitPoint;
            public uint leftNodeOffset;
        }

        public int nodeIndex;
        public bool isLeaf;
        public LeafData? leafData;
        public NodeData? nodeData;
    }
}