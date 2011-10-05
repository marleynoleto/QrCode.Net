namespace Gma.QrCodeNet.Encoding.Positioning
{
    internal class PositionDetectionPattern : PatternStencilBase
    {
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
    }
}
