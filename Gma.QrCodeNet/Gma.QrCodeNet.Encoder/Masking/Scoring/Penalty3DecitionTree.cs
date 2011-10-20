namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal class Penalty3DecitionTree
    {
        private BitBinaryTree<Penalty3DecitionNode> bitCheckTree = null;

        private const bool x = true;
        private const bool o = false;
        
        private const int ContinueCheck = -1;
        private const int PatternFound = 0;

        internal Penalty3DecitionTree()
        {
            this.initialTree();
        }

        private void initialTree()
        {
            bitCheckTree = new BitBinaryTree<Penalty3DecitionNode>();

            bitCheckTree.Root = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(true, 4, ContinueCheck));
            bitCheckTree.Root.One = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(x, 3, ContinueCheck));
            bitCheckTree.Root.Zero = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(o, 3, ContinueCheck));

            bitCheckTree.Root.One.One = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(x, 2, ContinueCheck));
            bitCheckTree.Root.One.Zero = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(o, 2, 3));

            bitCheckTree.Root.One.One.One = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(x, 1, ContinueCheck));
            bitCheckTree.Root.One.One.Zero = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(o, 1, 2));

            bitCheckTree.Root.One.One.One.One = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(x, 0, 5));
            bitCheckTree.Root.One.One.One.Zero = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(o, 0, 1));


            bitCheckTree.Root.Zero.One = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(x, 2, ContinueCheck));
            bitCheckTree.Root.Zero.Zero = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(o, 2, 4));

            bitCheckTree.Root.Zero.One.One = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(x, 1, ContinueCheck));
            bitCheckTree.Root.Zero.One.Zero = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(o, 1, 4));

            bitCheckTree.Root.Zero.One.One.One = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(x, 0, ContinueCheck));
            bitCheckTree.Root.Zero.One.One.Zero = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(o, 0, 4));

            bitCheckTree.Root.Zero.One.One.One.One = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(x, -1, 4));
            bitCheckTree.Root.Zero.One.One.One.Zero = new BitBinaryTreeNode<Penalty3DecitionNode>(new Penalty3DecitionNode(o, -1, PatternFound));

        }

        internal BitBinaryTreeNode<Penalty3DecitionNode> Root
        {
            get
            {
                if (bitCheckTree == null)
                    this.initialTree();
                return bitCheckTree.Root;
            }
        }
    }
}
