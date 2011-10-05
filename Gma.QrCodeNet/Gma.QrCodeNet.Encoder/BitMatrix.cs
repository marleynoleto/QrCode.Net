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

        internal void CopyTo(SimpleBitMatrix target, Rectangle sourceArea, Point targetPoint)
        {
            for (int j = 0; j < sourceArea.Size.Height; j++)
            {
                for (int i = 0; i < sourceArea.Size.Width; i++)
                {
                    bool value = this[sourceArea.Location.X + i, sourceArea.Location.Y + j];
                    target[targetPoint.X + i, targetPoint.Y + j] = value;
                }
            }
        }

        internal void CopyTo(SimpleBitMatrix target, Point targetPoint)
        {
            CopyTo(target, new Rectangle(new Point(0,0), new Size(Width, Height)), targetPoint);
        }
    }
}
