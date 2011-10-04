using System;

namespace Gma.QrCodeNet.Encoding.Masking
{
    internal class XorBitMatrix : BitMatrix
    {
        private readonly BitMatrix m_First;
        private readonly BitMatrix m_Second;

        public XorBitMatrix(BitMatrix first, BitMatrix second)
        {
            m_First = first;
            m_Second = second;
        }

        public override bool this[int i, int j]
        {
            get { return m_First[i, j] ^ m_Second[i, j]; }
            set { throw new NotSupportedException(); }
        }

        public override int Width
        {
            get { return m_First.Width; }
        }

        public override int Height
        {
            get { return m_First.Height; }
        }
    }
}
