using MTXEditorIO.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MTXEditorIO.Raw.Col
{
    public class ColBSPTree :IReadableWriteable
    {
        const int BSPNodeSize = 8; // all nodes are 8 bytes in size

        public TreeNode root = null!;

        public void ReadFrom(BinaryReader reader)
        {
            root = ReadTree(reader);
        }

        private TreeNode ReadTree(BinaryReader reader)
        {
            var node = new TreeNode();

            var binaryNode = ReadBinaryNode(reader);
            if (binaryNode.type == BSPSplitAxis.Leaf)
            {
                Console.WriteLine("found leaf");
                node.isLeaf = true;
                var d = node.leafData = new TreeNode.LeafData();
                d.numFaces = binaryNode.leaf.numFaces;
                d.nodeFaceIndexOffset = binaryNode.leaf.nodeFaceIndexOffset;
            }
            else
            {
                var d = node.nodeData = new TreeNode.NodeData();
                d.left = ReadTree(reader);
                d.right = ReadTree(reader);
                d.splitAxis = binaryNode.axisSplit.SplitAxis;
                d.splitPoint = binaryNode.axisSplit.SplitPoint;
                d.leftNodeIndex = binaryNode.axisSplit.leftNodeIndex; //unsure if we really need to store this
            }
            return node;
        }

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
    }
}
