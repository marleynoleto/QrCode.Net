namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal class Penalty2CheckTree
    {
        private BitBinaryTree<Penalty2NodeValue> bitCheckTree = null;

        private const int ContinueCheck = -1;
        private const int BlockFound = 0;

        internal Penalty2CheckTree()
        {
            this.initialTree();
        }

        private void initialTree()
        {
            Point TopRight = new Point(1, 0);
            Point BottomRight = new Point(1, 1);
            Point BottomLeft = new Point(0, 1);
            Point CurrentPos = new Point(0, 0);

            bitCheckTree = new BitBinaryTree<Penalty2NodeValue>();

            bitCheckTree.Root = new BitBinaryTreeNode<Penalty2NodeValue>(new Penalty2NodeValue(true, TopRight, ContinueCheck));
            bitCheckTree.Root.One = new BitBinaryTreeNode<Penalty2NodeValue>(new Penalty2NodeValue(true, BottomRight, ContinueCheck));
            bitCheckTree.Root.Zero = new BitBinaryTreeNode<Penalty2NodeValue>(new Penalty2NodeValue(false, BottomRight, ContinueCheck));

            bitCheckTree.Root.One.One = new BitBinaryTreeNode<Penalty2NodeValue>(new Penalty2NodeValue(true, BottomLeft, ContinueCheck));
            bitCheckTree.Root.One.Zero = new BitBinaryTreeNode<Penalty2NodeValue>(new Penalty2NodeValue(false, BottomLeft, 2));

            bitCheckTree.Root.One.One.One = new BitBinaryTreeNode<Penalty2NodeValue>(new Penalty2NodeValue(true, CurrentPos, BlockFound));
            bitCheckTree.Root.One.One.Zero = new BitBinaryTreeNode<Penalty2NodeValue>(new Penalty2NodeValue(false, CurrentPos, 1));

            bitCheckTree.Root.Zero.One = new BitBinaryTreeNode<Penalty2NodeValue>(new Penalty2NodeValue(true, BottomLeft, 2));
            bitCheckTree.Root.Zero.Zero = new BitBinaryTreeNode<Penalty2NodeValue>(new Penalty2NodeValue(false, BottomLeft, 1));

        }

        internal BitBinaryTreeNode<Penalty2NodeValue> Root
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
