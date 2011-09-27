using System;

namespace Gma.QrCodeNet.Encoding.Masking
{
    public abstract class Pattern : BitMatrix
    {
        public override int Width { get { throw new NotSupportedException(); } }

        public abstract MaskPatternType MaskPatternType { get; }
    }
}
