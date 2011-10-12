namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal class Penalty3CheckTree
    {
        private BitBinaryTree<Penalty3NodeValue> bitCheckTree = null;

        private const bool x = true;
        private const bool o = false;
        
        private const int ContinueCheck = -1;
        private const int PatternFound = 0;

        internal Penalty3CheckTree()
        {
            this.initialTree();
        }

        private void initialTree()
        {
            bitCheckTree = new BitBinaryTree<Penalty3NodeValue>();

            bitCheckTree.Root = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(true, 4, ContinueCheck));
            bitCheckTree.Root.One = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(x, 3, ContinueCheck));
            bitCheckTree.Root.Zero = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(o, 3, ContinueCheck));

            bitCheckTree.Root.One.One = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(x, 2, ContinueCheck));
            bitCheckTree.Root.One.Zero = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(o, 2, 3));

            bitCheckTree.Root.One.One.One = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(x, 1, ContinueCheck));
            bitCheckTree.Root.One.One.Zero = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(o, 1, 2));

            bitCheckTree.Root.One.One.One.One = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(x, 0, 5));
            bitCheckTree.Root.One.One.One.Zero = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(o, 0, 1));


            bitCheckTree.Root.Zero.One = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(x, 2, ContinueCheck));
            bitCheckTree.Root.Zero.Zero = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(o, 2, 4));

            bitCheckTree.Root.Zero.One.One = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(x, 1, ContinueCheck));
            bitCheckTree.Root.Zero.One.Zero = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(o, 1, 4));

            bitCheckTree.Root.Zero.One.One.One = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(x, 0, ContinueCheck));
            bitCheckTree.Root.Zero.One.One.Zero = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(o, 0, 4));

            bitCheckTree.Root.Zero.One.One.One.One = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(x, -1, 4));
            bitCheckTree.Root.Zero.One.One.One.Zero = new BitBinaryTreeNode<Penalty3NodeValue>(new Penalty3NodeValue(o, -1, PatternFound));

        }

        internal BitBinaryTreeNode<Penalty3NodeValue> Root
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
