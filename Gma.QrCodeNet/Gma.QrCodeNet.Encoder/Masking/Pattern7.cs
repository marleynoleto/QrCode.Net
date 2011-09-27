namespace Gma.QrCodeNet.Encoding.Masking
{
    internal class Pattern7 : Pattern
    {
        public override bool this[int i, int j]
        {
            get { return ((i * j) % 2 + (i * j) % 3) % 2 == 0; }
        }

        public override MaskPatternType MaskPatternType
        {
            get { return MaskPatternType.Type7; }
        }
    }
}
