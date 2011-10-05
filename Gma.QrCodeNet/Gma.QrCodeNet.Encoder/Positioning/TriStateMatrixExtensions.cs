using System.Collections.Generic;

namespace Gma.QrCodeNet.Encoding.Positioning
{
    internal static class TriStateMatrixExtensions
    {
        internal static TriStateMatrix Embed(this TriStateMatrix matrix,BitMatrix stencil, Point location)
        {
            stencil.CopyTo(matrix, location);
            return matrix;
        }

        internal static TriStateMatrix Embed(this TriStateMatrix matrix, BitMatrix stencil, IEnumerable<Point> locations)
        {
            foreach (Point location in locations)
            {
                Embed(matrix, stencil, location);
            }
            return matrix;
        }
    }
}
