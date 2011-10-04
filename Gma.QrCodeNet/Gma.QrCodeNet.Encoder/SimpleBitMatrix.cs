using System.Collections;

namespace Gma.QrCodeNet.Encoding
{
    public class SimpleBitMatrix : SquareBitMatrix
    {
        private readonly BitArray m_InternalArray;

        public SimpleBitMatrix(int width)
            : base(width)
        {
            m_InternalArray = new BitArray(width*width);
        }

        public override bool this[int i, int j]
        {
            get
            {
                return m_InternalArray[PointToIndex(j, i)];
            }
            set
            {
                m_InternalArray[PointToIndex(j, i)] = value;    
            }
        }

        private int PointToIndex(int j, int i)
        {
            return j * Width + i;
        }
    }
}