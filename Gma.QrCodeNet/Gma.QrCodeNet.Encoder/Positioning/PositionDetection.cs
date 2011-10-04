using com.google.zxing;

namespace Gma.QrCodeNet.Encoding.Positioning
{
    internal static class PositioninngPatternBuilder
    {
        private static readonly int[][] s_PositionDetectionPattern = new int[][]
                                                                         {
                                                                             new int[] { 1, 1, 1, 1, 1, 1, 1 }, 
                                                                             new int[] { 1, 0, 0, 0, 0, 0, 1 }, 
                                                                             new int[] { 1, 0, 1, 1, 1, 0, 1 }, 
                                                                             new int[] { 1, 0, 1, 1, 1, 0, 1 }, 
                                                                             new int[] { 1, 0, 1, 1, 1, 0, 1 }, 
                                                                             new int[] { 1, 0, 0, 0, 0, 0, 1 }, 
                                                                             new int[] { 1, 1, 1, 1, 1, 1, 1 }
                                                                         };

        private static readonly int[][] s_HorizontalSeparationPattern = new int[][] { new int[] { 0, 0, 0, 0, 0, 0, 0, 0 } };

        private static readonly int[][] s_VerticalSeparationPattern = new int[][] { new int[] { 0 }, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }, new int[] { 0 } };


        // Embed basic patterns. On success, modify the matrix and return true.
        // The basic patterns are:
        // - Position detection patterns
        // - Timing patterns
        // - Dark dot at the left bottom corner
        // - Position adjustment patterns, if need be
        internal static void EmbedBasicPatterns(int version, TriStateMatrix matrix)
        {
            // Let's get started with embedding big squares at corners.
            embedPositionDetectionPatternsAndSeparators(matrix);
            // Then, embed the dark dot at the left bottom corner.
            embedDarkDotAtLeftBottomCorner(matrix);

            // Position adjustment patterns appear if version >= 2.
            PositionAdjustment.maybeEmbedPositionAdjustmentPatterns(version, matrix);
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
                    matrix.Set(6, i,value);
                }

                // Vertical line.
                if (!matrix.IsUsed(i, 6))
                {
                    matrix.Set(i, 6, value);
                }
            }
        }

        // Embed the lonely dark dot at left bottom corner. JISX0510:2004 (p.46)
        private static void embedDarkDotAtLeftBottomCorner(TriStateMatrix matrix)
        {
            matrix.Set(8, matrix.Width - 8, true);
        }

        private static void embedHorizontalSeparationPattern(int xStart, int yStart, TriStateMatrix matrix)
        {
            for (int x = 0; x < 8; ++x)
            {
                matrix.Set(yStart, xStart + x, (sbyte)s_HorizontalSeparationPattern[0][x]==1);
            }
        }

        private static void embedVerticalSeparationPattern(int xStart, int yStart, TriStateMatrix matrix)
        {
            for (int y = 0; y < 7; ++y)
            {
                matrix.Set(yStart + y, xStart, (sbyte)s_VerticalSeparationPattern[y][0]==1);
            }
        }

        // Note that we cannot unify the function with embedPositionDetectionPattern() despite they are
        // almost identical, since we cannot write a function that takes 2D arrays in different sizes in
        // C/C++. We should live with the fact.

        private static void embedPositionDetectionPattern(int xStart, int yStart, TriStateMatrix matrix)
        {
            for (int y = 0; y < 7; ++y)
            {
                for (int x = 0; x < 7; ++x)
                {
                    matrix.Set(yStart + y, xStart + x, (sbyte)s_PositionDetectionPattern[y][x]==1);
                }
            }
        }

        // Embed position detection patterns and surrounding vertical/horizontal separators.
        private static void embedPositionDetectionPatternsAndSeparators(TriStateMatrix matrix)
        {
            // Embed three big squares at corners.
            int pdpWidth = s_PositionDetectionPattern[0].Length;
            // Left top corner.
            embedPositionDetectionPattern(0, 0, matrix);
            // Right top corner.
            embedPositionDetectionPattern(matrix.Width - pdpWidth, 0, matrix);
            // Left bottom corner.
            embedPositionDetectionPattern(0, matrix.Width - pdpWidth, matrix);

            // Embed horizontal separation patterns around the squares.
            int hspWidth = s_HorizontalSeparationPattern[0].Length;
            // Left top corner.
            embedHorizontalSeparationPattern(0, hspWidth - 1, matrix);
            // Right top corner.
            embedHorizontalSeparationPattern(matrix.Width - hspWidth, hspWidth - 1, matrix);
            // Left bottom corner.
            embedHorizontalSeparationPattern(0, matrix.Width - hspWidth, matrix);

            // Embed vertical separation patterns around the squares.
            int vspSize = s_VerticalSeparationPattern.Length;
            // Left top corner.
            embedVerticalSeparationPattern(vspSize, 0, matrix);
            // Right top corner.
            embedVerticalSeparationPattern(matrix.Width - vspSize - 1, 0, matrix);
            // Left bottom corner.
            embedVerticalSeparationPattern(vspSize, matrix.Width - vspSize, matrix);
        }

        // Embed position adjustment patterns if need be.
    }
}