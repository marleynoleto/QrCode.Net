using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gma.QrCodeNet.Encoding.Positioning
{
    class PositionAdjustment
    {
        private static readonly int[][] s_PositionAdjustmentPattern = new[]
                                                                          {
                                                                              new [] { 1, 1, 1, 1, 1 }, 
                                                                              new [] { 1, 0, 0, 0, 1 }, 
                                                                              new [] { 1, 0, 1, 0, 1 }, 
                                                                              new [] { 1, 0, 0, 0, 1 }, 
                                                                              new [] { 1, 1, 1, 1, 1 }
                                                                          };


        private static readonly byte[][] s_PositionAdjustmentPatternCoordinatesByVersion = new []
                                                                                         {
                                                                                             null,
                                                                                             new byte[] {} , 
                                                                                             new byte[] { 6, 18}, 
                                                                                             new byte[] { 6, 22 }, 
                                                                                             new byte[] { 6, 26 }, 
                                                                                             new byte[] { 6, 30 }, 
                                                                                             new byte[] { 6, 34 }, 
                                                                                             new byte[] { 6, 22, 38 }, 
                                                                                             new byte[] { 6, 24, 42 }, 
                                                                                             new byte[] { 6, 26, 46 }, 
                                                                                             new byte[] { 6, 28, 50 }, 
                                                                                             new byte[] { 6, 30, 54 }, 
                                                                                             new byte[] { 6, 32, 58 }, 
                                                                                             new byte[] { 6, 34, 62 }, 
                                                                                             new byte[] { 6, 26, 46, 66 }, 
                                                                                             new byte[] { 6, 26, 48, 70 }, 
                                                                                             new byte[] { 6, 26, 50, 74 }, 
                                                                                             new byte[] { 6, 30, 54, 78 }, 
                                                                                             new byte[] { 6, 30, 56, 82 }, 
                                                                                             new byte[] { 6, 30, 58, 86 }, 
                                                                                             new byte[] { 6, 34, 62, 90 }, 
                                                                                             new byte[] { 6, 28, 50, 72, 94 }, 
                                                                                             new byte[] { 6, 26, 50, 74, 98 }, 
                                                                                             new byte[] { 6, 30, 54, 78, 102 }, 
                                                                                             new byte[] { 6, 28, 54, 80, 106 }, 
                                                                                             new byte[] { 6, 32, 58, 84, 110 }, 
                                                                                             new byte[] { 6, 30, 58, 86, 114 }, 
                                                                                             new byte[] { 6, 34, 62, 90, 118 }, 
                                                                                             new byte[] { 6, 26, 50, 74, 98, 122 }, 
                                                                                             new byte[] { 6, 30, 54, 78, 102, 126 }, 
                                                                                             new byte[] { 6, 26, 52, 78, 104, 130 }, 
                                                                                             new byte[] { 6, 30, 56, 82, 108, 134 }, 
                                                                                             new byte[] { 6, 34, 60, 86, 112, 138 }, 
                                                                                             new byte[] { 6, 30, 58, 86, 114, 142 }, 
                                                                                             new byte[] { 6, 34, 62, 90, 118, 146 }, 
                                                                                             new byte[] { 6, 30, 54, 78, 102, 126, 150 }, 
                                                                                             new byte[] { 6, 24, 50, 76, 102, 128, 154 }, 
                                                                                             new byte[] { 6, 28, 54, 80, 106, 132, 158 }, 
                                                                                             new byte[] { 6, 32, 58, 84, 110, 136, 162 }, 
                                                                                             new byte[] { 6, 26, 54, 82, 110, 138, 166 }, 
                                                                                             new byte[] { 6, 30, 58, 86, 114, 142, 170 }
                                                                                         };

        internal PositionAdjustment(byte[] coordinates)
        {
            
        }

        private static void embedPositionAdjustmentPattern(int xStart, int yStart, TriStateMatrix matrix)
        {
            for (int y = 0; y < 5; ++y)
            {
                for (int x = 0; x < 5; ++x)
                {
                    matrix.Set(yStart + y, xStart + x, (sbyte)s_PositionAdjustmentPattern[y][x]==1);
                }
            }
        }

        internal static void maybeEmbedPositionAdjustmentPatterns(int version, TriStateMatrix matrix)
        {
            byte[] coordinates = s_PositionAdjustmentPatternCoordinatesByVersion[version];
            foreach (byte x in coordinates)
            {
                foreach (byte y in coordinates)
                {
                    // If the cell is unset, we embed the position adjustment pattern here.
                    if (!matrix.IsUsed(x, y))
                    {
                        // -2 is necessary since the x/y coordinates point to the center of the pattern, not the
                        // left top corner.
                        embedPositionAdjustmentPattern(y - 2, x - 2, matrix);
                    }
                }
            }
        }
    }
}
