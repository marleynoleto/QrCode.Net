namespace Gma.QrCodeNet.Encoding.Positioning
{
    internal class VerticalSeparationPattern : PatternStencilBase
    {

        private static readonly bool[,] s_VerticalSeparationPattern = new [,] { { o, o, o, o, o, o, o, o } };

        public override bool[,] Stencil
        {
            get { return s_VerticalSeparationPattern; }
        }
    }
}
