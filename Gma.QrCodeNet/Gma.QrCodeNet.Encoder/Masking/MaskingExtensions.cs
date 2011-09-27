namespace Gma.QrCodeNet.Encoding.Masking
{
    public static class MaskingExtensions
    {
        public static BitMatrix Xor(this BitMatrix first, BitMatrix second)
        {
            return new XorBitMatrix(first, second);
        }

        public static BitMatrix Apply(this BitMatrix matrix, Pattern pattern)
        {
            return matrix.Xor(pattern);
        }

        public static BitMatrix Apply(this Pattern pattern, BitMatrix matrix)
        {
            return matrix.Xor(pattern);
        }
    }
}
