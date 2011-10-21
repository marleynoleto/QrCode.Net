namespace Gma.QrCodeNet.Encoding
{
    public abstract class BitMatrix
    {
        public abstract bool this[int i, int j] { get; set; }
        public abstract int Width { get; }
        public abstract int Height { get; }

        internal MatrixSize Size
        {
            get { return new MatrixSize(Width, Height); }
        }

        internal bool this[MatrixPoint point]
        {
            get { return this[point.X, point.Y]; }
            set { this[point.X, point.Y] = value; }
        }

        internal void CopyTo(SimpleBitMatrix target, MatrixRectangle sourceArea, MatrixPoint targetPoint)
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

        internal void CopyTo(SimpleBitMatrix target, MatrixPoint targetPoint)
        {
            CopyTo(target, new MatrixRectangle(new MatrixPoint(0,0), new MatrixSize(Width, Height)), targetPoint);
        }
    }
}
