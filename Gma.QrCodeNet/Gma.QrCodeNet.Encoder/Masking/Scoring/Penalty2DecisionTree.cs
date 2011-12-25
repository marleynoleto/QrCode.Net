namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal class Penalty2DecisionTree
    {
        private BitBinaryTree<Penalty2DecisionNode> bitCheckTree = null;

        private const int ContinueCheck = -1;
        private const int BlockFound = 0;

        internal Penalty2DecisionTree()
        {
            this.initialTree();
        }

        private void initialTree()
        {
            MatrixPoint TopRight = new MatrixPoint(1, 0);
            MatrixPoint BottomRight = new MatrixPoint(1, 1);
            MatrixPoint BottomLeft = new MatrixPoint(0, 1);
            MatrixPoint CurrentPos = new MatrixPoint(0, 0);

            bitCheckTree = new BitBinaryTree<Penalty2DecisionNode>();

            bitCheckTree.Root = new BitBinaryTreeNode<Penalty2DecisionNode>(new Penalty2DecisionNode(true, TopRight, ContinueCheck));
            bitCheckTree.Root.One = new BitBinaryTreeNode<Penalty2DecisionNode>(new Penalty2DecisionNode(true, BottomRight, ContinueCheck));
            bitCheckTree.Root.Zero = new BitBinaryTreeNode<Penalty2DecisionNode>(new Penalty2DecisionNode(false, BottomRight, ContinueCheck));

            bitCheckTree.Root.One.One = new BitBinaryTreeNode<Penalty2DecisionNode>(new Penalty2DecisionNode(true, BottomLeft, ContinueCheck));
            bitCheckTree.Root.One.Zero = new BitBinaryTreeNode<Penalty2DecisionNode>(new Penalty2DecisionNode(false, BottomLeft, 2));

            bitCheckTree.Root.One.One.One = new BitBinaryTreeNode<Penalty2DecisionNode>(new Penalty2DecisionNode(true, CurrentPos, BlockFound));
            bitCheckTree.Root.One.One.Zero = new BitBinaryTreeNode<Penalty2DecisionNode>(new Penalty2DecisionNode(false, CurrentPos, 1));

            bitCheckTree.Root.Zero.One = new BitBinaryTreeNode<Penalty2DecisionNode>(new Penalty2DecisionNode(true, BottomLeft, 2));
            bitCheckTree.Root.Zero.Zero = new BitBinaryTreeNode<Penalty2DecisionNode>(new Penalty2DecisionNode(false, BottomLeft, 1));

        }

        internal BitBinaryTreeNode<Penalty2DecisionNode> Root
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
