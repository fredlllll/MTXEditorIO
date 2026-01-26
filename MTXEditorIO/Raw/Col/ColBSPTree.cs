using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Col
{
    public class ColBSPTree : IReadableWriteable
    {
        private class Frame { public TreeNode Node; public long Offset; public bool LeftDone; public bool RightDone; }

        const int BSPNodeSize = 8; // all nodes are 8 bytes in size

        public TreeNode root = null!;

        public void ReadFrom(BinaryReader reader)
        {
            root = ReadTreeIterative(reader);
        }

        public TreeNode ReadTreeIterative(BinaryReader reader)
        {
            long rootOffset = reader.BaseStream.Position;

            var root = new TreeNode();
            var stack = new Stack<Frame>();

            stack.Push(new Frame { Node = root, Offset = rootOffset });

            while (stack.Count > 0)
            {
                var frame = stack.Peek();
                reader.BaseStream.Position = frame.Offset;

                var binaryNode = ReadBinaryNode(reader);

                if (binaryNode.type == BSPSplitAxis.Leaf)
                {
                    frame.Node.isLeaf = true;
                    frame.Node.leafData = new TreeNode.LeafData
                    {
                        numFaces = binaryNode.leaf.numFaces,
                        nodeFaceIndexOffset = binaryNode.leaf.nodeFaceIndexOffset
                    };

                    frame.Node.serializedSize = BSPNodeSize;
                    stack.Pop();
                }
                else if (!frame.LeftDone)
                {
                    frame.Node.isLeaf = false;
                    frame.Node.nodeData = new TreeNode.NodeData
                    {
                        splitAxis = binaryNode.axisSplit.SplitAxis,
                        splitPoint = binaryNode.axisSplit.SplitPoint,
                        leftNodeIndex = binaryNode.axisSplit.leftNodeOffset
                    };

                    long leftOffset = binaryNode.axisSplit.leftNodeOffset;// rootOffset + (binaryNode.axisSplit.leftNodeIndex * BSPNodeSize);

                    frame.Node.nodeData.left = new TreeNode();
                    stack.Push(new Frame { Node = frame.Node.nodeData.left, Offset = leftOffset });

                    frame.LeftDone = true;
                }
                else if (!frame.RightDone)
                {
                    long rightOffset = frame.Offset + BSPNodeSize + frame.Node.nodeData.left.serializedSize;

                    frame.Node.nodeData.right = new TreeNode();
                    stack.Push(new Frame { Node = frame.Node.nodeData.right, Offset = rightOffset });

                    frame.RightDone = true;
                }
                else
                {

                    // Both children done → compute size and pop
                    frame.Node.serializedSize =
                        BSPNodeSize +
                        frame.Node.nodeData.left.serializedSize +
                        frame.Node.nodeData.right.serializedSize;

                    stack.Pop();
                }
            }

            return root;
        }


        /*public TreeNode ReadTreeIterative(BinaryReader reader)
        {
            long rootOffset = reader.BaseStream.Position;

            var root = new TreeNode();
            var stack = new Stack<Frame>();

            stack.Push(new Frame { Node = root, Offset = rootOffset });

            while (stack.Count > 0)
            {
                var frame = stack.Peek();
                reader.BaseStream.Position = frame.Offset;

                var binaryNode = ReadBinaryNode(reader);

                if (binaryNode.type == BSPSplitAxis.Leaf)
                {
                    frame.Node.isLeaf = true;
                    frame.Node.leafData = new TreeNode.LeafData
                    {
                        numFaces = binaryNode.leaf.numFaces,
                        nodeFaceIndexOffset = binaryNode.leaf.nodeFaceIndexOffset
                    };

                    frame.Node.serializedSize = 8;
                    stack.Pop();
                    continue;
                }
                else if (!frame.LeftDone)
                {
                    frame.Node.isLeaf = false;
                    frame.Node.nodeData = new TreeNode.NodeData
                    {
                        splitAxis = binaryNode.axisSplit.SplitAxis,
                        splitPoint = binaryNode.axisSplit.SplitPoint,
                        leftNodeIndex = binaryNode.axisSplit.leftNodeIndex
                    };

                    long leftOffset = rootOffset + (binaryNode.axisSplit.leftNodeIndex * BSPNodeSize);

                    frame.Node.nodeData.left = new TreeNode();
                    stack.Push(new Frame { Node = frame.Node.nodeData.left, Offset = leftOffset });

                    frame.LeftDone = true;
                    continue;
                }
                else
                {
                    // Left subtree done → compute right offset
                    long rightOffset = frame.Offset + 8 + frame.Node.nodeData.left.serializedSize;

                    frame.Node.nodeData.right = new TreeNode();
                    stack.Push(new Frame { Node = frame.Node.nodeData.right, Offset = rightOffset });

                    // After pushing right child, mark this frame as complete
                    stack.Pop();
                }
            }

            return root;
        }*/


        private ColBSPNode ReadBinaryNode(BinaryReader reader)
        {
            ColBSPNode node = new ColBSPNode();
            node.ReadFrom(reader);
            return node;
        }

        public void WriteTo(BinaryWriter writer)
        {
            throw new NotImplementedException();
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
            public uint leftNodeIndex;
        }

        public bool isLeaf;
        public LeafData? leafData;
        public NodeData? nodeData;
        public uint serializedSize;
    }
}
