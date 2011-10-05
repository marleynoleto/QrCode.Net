namespace Gma.QrCodeNet.Encoding.Positioning
{
    internal class HorizontalSeparationPattern : PatternStencilBase
    {

        private static readonly bool[,] s_HorizontalSeparationPattern = new [,]
                                                                          {
                                                                              { o }, 
                                                                              { o }, 
                                                                              { o }, 
                                                                              { o }, 
                                                                              { o }, 
                                                                              { o }, 
                                                                              { o }
                                                                          };

        public override bool[,] Stencil
        {
            get { return s_HorizontalSeparationPattern; }
        }
    }
}
