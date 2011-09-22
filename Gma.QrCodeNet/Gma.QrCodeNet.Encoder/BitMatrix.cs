namespace Gma.QrCodeNet.Encoding
{
    public class BitMatrix
    {
        private readonly bool[,] m_InternalMatrix;

        public BitMatrix(int width)
        {
            m_InternalMatrix = new bool[width,width];
        }

        public bool this[int i, int j]
        {
            get { return m_InternalMatrix[i, j]; }
            internal set { m_InternalMatrix[i, j] = value; }
        }

        public int Width
        {
            get { return m_InternalMatrix.GetLength(0); }
        }
    }
}
