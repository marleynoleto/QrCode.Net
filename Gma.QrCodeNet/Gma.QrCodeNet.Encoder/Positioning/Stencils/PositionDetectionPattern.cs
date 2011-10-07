using System;
using System.Collections.Generic;

namespace Gma.QrCodeNet.Encoding.Positioning.Stencils
{
    internal class PositionDetectionPattern : PatternStencilBase
    {
        public PositionDetectionPattern(int version)
            : base(version)
        {
        }

        private static readonly bool[,] s_PositionDetection = new[,]
                                                                         {
                                                                             { o, o, o, o, o, o, o, o, o },
                                                                             { o, x, x, x, x, x, x, x, o }, 
                                                                             { o, x, o, o, o, o, o, x, o }, 
                                                                             { o, x, o, x, x, x, o, x, o }, 
                                                                             { o, x, o, x, x, x, o, x, o }, 
                                                                             { o, x, o, x, x, x, o, x, o }, 
                                                                             { o, x, o, o, o, o, o, x, o }, 
                                                                             { o, x, x, x, x, x, x, x, o },
                                                                             { o, o, o, o, o, o, o, o, o }
                                                                         };

        public override bool[,] Stencil
        {
            get { return s_PositionDetection; }
        }

        public override void ApplyTo(TriStateMatrix matrix)
        {
            Size size = GetSizeOfSquareWithSeparators();
            
            Point leftTopCorner = new Point(0, 0);
            this.CopyTo(matrix, new Rectangle(new Point(1, 1), size), leftTopCorner);

            Point rightTopCorner = new Point(matrix.Width - this.Width + 1, 0);
            this.CopyTo(matrix, new Rectangle(new Point(0, 1), size), rightTopCorner);


            Point leftBottomCorner = new Point(0, matrix.Width - this.Width + 1);
            this.CopyTo(matrix, new Rectangle(new Point(1, 0), size), leftBottomCorner);
        }

        private Size GetSizeOfSquareWithSeparators()
        {
            return new Size(this.Width - 1, this.Height - 1);
        }
    }
}
