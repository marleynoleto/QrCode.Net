using System;

namespace Gma.QrCodeNet.Encoding.Positioning
{
    internal class TriStateMatrix
    {
        private readonly TriStateBit[,] m_Matrix;

        public TriStateMatrix(int width)
        {
            m_Matrix = new TriStateBit[width,width];
        }

        public TriStateBit this[int i, int j]
        {
            get { return m_Matrix[i, j];}
            set { m_Matrix[i, j] = value; }
        }

        public int Width { get { return m_Matrix.GetLength(0); } }
    }
}
