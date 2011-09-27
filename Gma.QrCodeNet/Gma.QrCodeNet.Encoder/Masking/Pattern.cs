namespace Gma.QrCodeNet.Encoding.Masking
{
    public abstract class Pattern : BitMatrix
    {

        private readonly int m_Width;

        protected Pattern(int width)
        {
            m_Width = width;
        }

        public override int Width { get { return m_Width; } }

        public abstract MaskPatternType MaskPatternType { get; }
    }
}
