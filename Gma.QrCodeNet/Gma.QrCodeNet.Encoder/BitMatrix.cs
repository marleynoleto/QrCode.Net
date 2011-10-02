namespace Gma.QrCodeNet.Encoding
{
    public abstract class BitMatrix
    {
        public abstract bool this[int i, int j] { get;}
        public abstract int Width { get; }
    }
}
