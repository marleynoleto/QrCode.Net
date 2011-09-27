using System;

namespace Gma.QrCodeNet.Encoding.Masking
{
    internal class XorBitMatrix : BitMatrix
    {
        private readonly BitMatrix m_First;
        private readonly BitMatrix m_Second;

        public XorBitMatrix(BitMatrix first, BitMatrix second)
        {
            if (first.Width!=second.Width)
            {
                throw new ArgumentException("Matrices must have same size.");
            }
            m_First = first;
            m_Second = second;
        }

        public override bool this[int i, int j]
        {
            get { return m_First[i, j] ^ m_Second[i, j]; }
        }

        public override int Width
        {
            get { return m_First.Width; }
        }
    }
}
