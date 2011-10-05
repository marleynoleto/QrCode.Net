using com.google.zxing;

namespace Gma.QrCodeNet.Encoding.Positioning
{
    internal static class PositioninngPatternBuilder
    {
        // Embed basic patterns. On success, modify the matrix and return true.
        // The basic patterns are:
        // - Position detection patterns
        // - Timing patterns
        // - Dark dot at the left bottom corner
        // - Position adjustment patterns, if need be
        internal static void EmbedBasicPatterns(int version, TriStateMatrix matrix)
        {
            // Let's get started with embedding big squares at corners.
            EmbedThreeBigSquaresAtCorners(matrix);
            // Then, embed the dark dot at the left bottom corner.
            embedDarkDotAtLeftBottomCorner(matrix);

            // Position adjustment patterns appear if version >= 2.
            new AlignmentPatternBuilder(version).Embed(matrix);
            // Timing patterns should be embedded after position adj. patterns.
            embedTimingPatterns(matrix);
        }


        private static void embedTimingPatterns(TriStateMatrix matrix)
        {
            // -8 is for skipping position detection patterns (size 7), and two horizontal/vertical
            // separation patterns (size 1). Thus, 8 = 7 + 1.
            for (int i = 8; i < matrix.Width - 8; ++i)
            {   
                bool value = (sbyte)((i + 1) % 2)==1;
                // Horizontal line.
               
                if (!matrix.IsUsed(6, i))
                {
                    matrix[6, i] = value;
                }

                // Vertical line.
                if (!matrix.IsUsed(i, 6))
                {
                    matrix[i, 6] = value;
                }
            }
        }

        // Embed the lonely dark dot at left bottom corner. JISX0510:2004 (p.46)
        private static void embedDarkDotAtLeftBottomCorner(TriStateMatrix matrix)
        {
            matrix[8, matrix.Width - 8] = true;
        }


        // Embed position detection patterns and surrounding vertical/horizontal separators.

        private static void EmbedThreeBigSquaresAtCorners(TriStateMatrix matrix)
        {
            PositionDetectionPattern bigSqaure = new PositionDetectionPattern();

            Size sizeWithSeparation = new Size(bigSqaure.Width - 1, bigSqaure.Height - 1);
            Point leftTopCorner = new Point(0,0);
            bigSqaure.CopyTo(matrix, new Rectangle(new Point(1,1), sizeWithSeparation), leftTopCorner);

            Point rightTopCorner = new Point(matrix.Width - bigSqaure.Width + 1, 0);
            bigSqaure.CopyTo(matrix, new Rectangle(new Point(0, 1), sizeWithSeparation), rightTopCorner);


            Point leftBottomCorner = new Point(0, matrix.Width - bigSqaure.Width + 1);
            bigSqaure.CopyTo(matrix, new Rectangle(new Point(1, 0), sizeWithSeparation), leftBottomCorner);
        }
    }
}