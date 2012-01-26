using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gma.QrCodeNet.Encoding.Windows.WPF
{
    public class Renderer
    {
        private int m_ModuleSize;

        //private Brush m_DarkBrush;
        //private Brush m_LightBrush;

        private Color m_DarkColor;
        private Color m_LightColor;

        private int m_quietZoneModules = 4;

        public Renderer(int moduleSize)
            : this(moduleSize, Colors.Black, Colors.White)
        {
        }

        public Renderer(int moduleSize, Color darkColor, Color lightColor)
        {
            m_ModuleSize = moduleSize;
            m_DarkColor = darkColor;
            m_LightColor = lightColor;
        }

        public void Draw(DrawingContext drawContext, BitMatrix matrix)
        {
            this.Draw(drawContext, matrix, new Point(0, 0));
        }

        public void Draw(DrawingContext drawContext, BitMatrix matrix, Point offset)
        {
            if (matrix == null)
            {
                this.DrawingQuitZone(drawContext, 21, offset);
                return;
            }
            else
                this.DrawingQuitZone(drawContext, matrix.Width, offset);

            int padding = m_ModuleSize * m_quietZoneModules;

            int preX = -1;

            Brush darkBrush = new SolidColorBrush(m_DarkColor);
            
            for (int y = 0; y < matrix.Width; y++)
            {
                for (int x = 0; x < matrix.Width; x++)
                {
                    if (matrix[x, y])
                    {
                        if (preX == -1)
                            preX = x;
                        if (x == matrix.Width - 1)
                        {
                            Point modulePosition = new Point(preX * m_ModuleSize + padding + offset.X, y * m_ModuleSize + offset.Y);
                            Rect moduleArea = new Rect(modulePosition, new Size((x - preX + 1) * m_ModuleSize, m_ModuleSize));
                            drawContext.DrawRectangle(darkBrush, null, moduleArea);
                            preX = -1;
                        }
                    }
                    else if (!matrix[x, y] && preX != -1)
                    {
                        Point modulePosition = new Point(preX * m_ModuleSize + padding + offset.X, y * m_ModuleSize + offset.Y);
                        Rect moduleArea = new Rect(modulePosition, new Size((x - preX) * m_ModuleSize, m_ModuleSize));
                        drawContext.DrawRectangle(darkBrush, null, moduleArea);
                        preX = -1;
                    }
                }
            }

        }

        public void Draw(WriteableBitmap wBitmap, BitMatrix matrix)
        {
            this.Draw(wBitmap, matrix, new Point(0, 0));
        }


        public void Draw(WriteableBitmap wBitmap, BitMatrix matrix, Point offset)
        {

            if (matrix == null)
            {
                this.DrawingQuitZone(wBitmap, 21, offset);
                return;
            }
            this.DrawingQuitZone(wBitmap, matrix.Width, offset);

            this.DrawDarkModule(wBitmap, matrix, offset);

        }

        private void DrawingQuitZone(WriteableBitmap wBitmap, int matrixWidth, Point offset)
        {
            int rectWidth = m_ModuleSize * (matrixWidth + (m_quietZoneModules * 2));
            wBitmap.FillRectangle(new Int32Rect((int)offset.X, (int)offset.Y, rectWidth, rectWidth), m_LightColor);
        }

        internal void DrawDarkModule(WriteableBitmap wBitmap, BitMatrix matrix, Point offset)
        {
            int padding = m_ModuleSize * m_quietZoneModules;

            int preX = -1;

            for (int y = 0; y < matrix.Width; y++)
            {
                for (int x = 0; x < matrix.Width; x++)
                {
                    if (matrix[x, y])
                    {
                        if (preX == -1)
                            preX = x;
                        if (x == matrix.Width - 1)
                        {
                            Int32Rect moduleArea =
                                new Int32Rect((int)(preX * m_ModuleSize + padding + offset.X), (int)(y * m_ModuleSize + offset.Y), (x - preX + 1) * m_ModuleSize, m_ModuleSize);
                            wBitmap.FillRectangle(moduleArea, m_DarkColor);
                            preX = -1;
                        }
                    }
                    else if (!matrix[x, y] && preX != -1)
                    {
                        Int32Rect moduleArea =
                            new Int32Rect((int)(preX * m_ModuleSize + padding + offset.X), (int)(y * m_ModuleSize + offset.Y), (x - preX) * m_ModuleSize, m_ModuleSize);
                        wBitmap.FillRectangle(moduleArea, m_DarkColor);
                        preX = -1;
                    }
                }
            }
        }

        private void DrawingQuitZone(DrawingContext drawContext, int matrixWidth, Point offset)
        {
            Brush lightBrush = new SolidColorBrush(m_LightColor);
            int rectWidth = m_ModuleSize * (matrixWidth + (m_quietZoneModules * 2));
            drawContext.DrawRectangle(lightBrush, null, new Rect(offset.X, offset.Y, rectWidth, rectWidth));
        }


        public int Measure(int matrixWidth)
        {
            int totalWidth = (m_quietZoneModules * 2 + matrixWidth) * m_ModuleSize;
            return totalWidth + 1;
        }

        public QuietZoneModules QuietZoneModules
        {
            get
            {
                return (QuietZoneModules)m_quietZoneModules;
            }
            set
            {
                m_quietZoneModules = (int)value;
            }
        }

        public int ModuleSize
        {
            get
            {
                return m_ModuleSize;
            }
            set
            {
                if (value > 0)
                    m_ModuleSize = value;
                else
                    throw new ArgumentOutOfRangeException("ModuleSize must be bigger than Zero");
            }
        }

        public Color DarkBrush
        {
            get
            {
                return m_DarkColor;
            }
            set
            {
                m_DarkColor = value;
            }
        }

        public Color LightBrush
        {
            get
            {
                return m_LightColor;
            }
            set
            {
                m_LightColor = value;
            }
        }

    }
}
