using System;
using System.Windows.Media;

namespace Gma.QrCodeNet.Encoding.Windows.Render
{
    public class EPSMediaColor : EPSColor
    {
        public Color Color { get; set; }

        public EPSMediaColor(Color color)
        {
            Color = color;
        }

        public override byte A
        {
            get
            {
                return Color.A;
            }
        }

        public override byte B
        {
            get { return Color.B; }
        }

        public override byte G
        {
            get { return Color.G; }
        }

        public override byte R
        {
            get { return Color.R; }
        }
    }
}
