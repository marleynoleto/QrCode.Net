using System.Collections;

namespace Gma.QrCodeNet.Encoding
{
    public class SimpleBitMatrix : SquareBitMatrix
    {
        private readonly BitArray m_InternalArray;
        //private readonly bool[,] m_InternalArray;

        public SimpleBitMatrix(int width)
            : base(width)
        {
            m_InternalArray = new BitArray(width*width);
            //m_InternalArray = new bool[width, width];
        }

        public override bool this[int i, int j]
        {
            get
            {
                return m_InternalArray[PointToIndex(j, i)];
                //return m_InternalArray[i, j];
            }
            set
            {
                m_InternalArray[PointToIndex(j, i)] = value;    
                //m_InternalArray[i, j] = value;
            }
        }

        private int PointToIndex(int j, int i)
        {
            return j * Width + i;
        }
    }
}