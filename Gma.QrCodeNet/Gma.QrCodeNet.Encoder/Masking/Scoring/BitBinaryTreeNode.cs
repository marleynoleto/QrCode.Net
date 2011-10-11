namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal class BitBinaryTreeNode<T>
    {
        private T data;
        private BitBinaryTreeNode<T> OneNode = null;
        private BitBinaryTreeNode<T> ZeroNode = null;


        internal BitBinaryTreeNode() { }
        internal BitBinaryTreeNode(T data) : this(data, null, null) { }
        internal BitBinaryTreeNode(T data, BitBinaryTreeNode<T> one, BitBinaryTreeNode<T> zero)
        {
            Value = data;
            OneNode = one;
            ZeroNode = zero;
        }

        internal T Value
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        internal BitBinaryTreeNode<T> One
        {
            get
            {
                return OneNode;
            }
            set
            {
                OneNode = value;
            }
        }


        internal BitBinaryTreeNode<T> Zero
        {
            get
            {
                return ZeroNode;
            }
            set
            {
                ZeroNode = value;
            }
        }
    }
}
