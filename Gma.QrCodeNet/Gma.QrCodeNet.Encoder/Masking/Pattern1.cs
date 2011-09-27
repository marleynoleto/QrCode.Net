namespace Gma.QrCodeNet.Encoding.Masking
{
    internal class Pattern1 : Pattern
{
        public Pattern1(int width) 
            : base(width)
        {
        }

        public override bool this[int i, int j]
        {
            get { return (j + i) % 2 == 0; }
        }

        public override MaskPatternType MaskPatternType
        {
            get { return MaskPatternType.Type1; }
        }
}
}
