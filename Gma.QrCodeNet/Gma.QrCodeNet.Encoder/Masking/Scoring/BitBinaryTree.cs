namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal class BitBinaryTree<T>
    {
        private BitBinaryTreeNode<T> root;

        internal BitBinaryTree()
        {
            root = null;
        }

        internal virtual void Clear()
        {
            root = null;
        }

        internal BitBinaryTreeNode<T> Root
        {
            get
            {
                return root;
            }
            set
            {
                root = value;
            }
        }
    }
}
