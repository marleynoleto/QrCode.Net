namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal class BitCheckBinaryTree
    {
        private BitBinaryTree<BitCheckValue> bitCheckTree = null;

        private const bool x = true;
        private const bool o = false;

        internal BitCheckBinaryTree()
        {
            this.initialTree();
        }

        private void initialTree()
        {
            bitCheckTree = new BitBinaryTree<BitCheckValue>();

            bitCheckTree.Root = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(true, 4, -1));
            bitCheckTree.Root.One = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(x, 3, -1));
            bitCheckTree.Root.Zero = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(o, 3, -1));

            bitCheckTree.Root.One.One = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(x, 2, -1));
            bitCheckTree.Root.One.Zero = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(o, 2, 3));

            bitCheckTree.Root.One.One.One = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(x, 1, -1));
            bitCheckTree.Root.One.One.Zero = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(o, 1, 2));

            bitCheckTree.Root.One.One.One.One = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(x, 0, 5));
            bitCheckTree.Root.One.One.One.Zero = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(o, 0, 1));


            bitCheckTree.Root.Zero.One = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(x, 2, -1));
            bitCheckTree.Root.Zero.Zero = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(o, 2, 4));

            bitCheckTree.Root.Zero.One.One = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(x, 1, -1));
            bitCheckTree.Root.Zero.One.Zero = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(o, 1, 4));

            bitCheckTree.Root.Zero.One.One.One = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(x, 0, -1));
            bitCheckTree.Root.Zero.One.One.Zero = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(o, 0, 4));

            bitCheckTree.Root.Zero.One.One.One.One = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(x, -1, 4));
            bitCheckTree.Root.Zero.One.One.One.Zero = new BitBinaryTreeNode<BitCheckValue>(new BitCheckValue(o, -1, 0));

        }

        internal BitBinaryTreeNode<BitCheckValue> Root
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
