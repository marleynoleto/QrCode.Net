using System;
using System.Collections;
using System.Collections.Generic;

namespace Gma.QrCodeNet.Encoding
{
    internal struct Rectangle : IEnumerable<Point>
    {
        public Point Location { get; private set; }
        public Size Size { get; private set; }

        internal Rectangle(Point location, Size size) :
            this()
        {
            Location = location;
            Size = size;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            for (int j = Location.Y; j < Location.Y + Size.Height; j++)
            {
                for (int i = Location.X; i < Location.X + Size.Width; i++)
                {
                    yield return new Point(i, j);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
