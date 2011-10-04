namespace Gma.QrCodeNet.Encoding
{
    public abstract class SquareBitMatrix : BitMatrix
    {
        private readonly int m_Width;

        protected SquareBitMatrix(int width)
        {
            m_Width = width;
        }

        public override int Height
        {
            get { return Width; }
        }

        public override int Width
        {
            get { return m_Width; }
        }
    }
}
