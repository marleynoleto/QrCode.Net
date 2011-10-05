namespace Gma.QrCodeNet.Encoding
{
    internal struct Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        internal Point(int x, int y) 
            : this()
        {
            X = x;
            Y = y;
        }

        public Point Offset(Point offset)
        {
            return new Point(offset.X + this.X, offset.Y + this.Y);
        }

        internal Point Offset(int offsetX, int offsetY)
        {
            return Offset(new Point(offsetX, offsetY));
        }

        public override string ToString()
        {
            return string.Format("Point({0};{1})", X, Y);
        }
    }
}
