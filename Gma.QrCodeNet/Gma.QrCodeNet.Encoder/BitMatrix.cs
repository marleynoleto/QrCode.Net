namespace Gma.QrCodeNet.Encoding
{
    public abstract class BitMatrix
    {
        public abstract bool this[int i, int j] { get; set; }
        public abstract int Width { get; }
        public abstract int Height { get; }

        internal Size Size
        {
            get { return new Size(Width, Height); }
        }

        internal bool this[Point point]
        {
            get { return this[point.X, point.Y]; }
            set { this[point.X, point.Y] = value; }
        }

        internal void CopyTo(SimpleBitMatrix target, Rectangle sourceArea, Point targetOffset)
        {
            foreach (Point point in sourceArea)
            {
                bool sourceValue = this[point];
                target[point.Offset(targetOffset)] = sourceValue;
            }
        }

        internal void CopyTo(SimpleBitMatrix target, Point targetOffset)
        {
            CopyTo(target, new Rectangle(new Point(0,0), new Size(Width, Height)), targetOffset);
        }

    }
}
