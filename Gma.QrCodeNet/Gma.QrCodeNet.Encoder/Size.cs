namespace Gma.QrCodeNet.Encoding
{
    internal struct Size
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        internal Size(int width, int height) 
            : this()
        {
            Width = width;
            Height = height;
        }
    }
}
