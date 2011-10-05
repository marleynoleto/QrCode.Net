namespace Gma.QrCodeNet.Encoding.Positioning
{
    internal class AlignmentPattern : PatternStencilBase
    {

        private static readonly bool[,] s_AlignmentPattern = new[,]
                                                                          {
                                                                              { x, x, x, x, x }, 
                                                                              { x, o, o, o, x }, 
                                                                              { x, o, x, o, x }, 
                                                                              { x, o, o, o, x }, 
                                                                              { x, x, x, x, x }
                                                                          };

        public override bool[,] Stencil
        {
            get { return s_AlignmentPattern; }
        }
    }
}
