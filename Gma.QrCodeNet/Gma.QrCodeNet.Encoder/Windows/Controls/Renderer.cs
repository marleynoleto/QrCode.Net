using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Gma.QrCodeNet.Encoding;

namespace Gma.QrCodeNet.Encoding.Windows.Controls
{
    public class Renderer
    {
        private int m_ModuleSize;
        private Brush m_DarkBrush;
        private Brush m_LightBrush;
        private int m_Padding;

        private int quietZoneModules = 4;
        
        public Renderer(int moduleSize)
            : this(moduleSize, Brushes.Black, Brushes.White)
        {
        }

        public Renderer(int moduleSize, Brush darkBrush, Brush lightBrush)
        {
            m_ModuleSize = moduleSize;
            m_DarkBrush = darkBrush;
            m_LightBrush = lightBrush;
        }

        public void Draw(Graphics graphics, BitMatrix matrix)
        {
            this.Draw(graphics, matrix, new Point(0,0));
        }

        public void Draw(Graphics graphics, BitMatrix matrix, Point offset)
        {
        	if(matrix == null)
        	{
        		DrawQuietZone(graphics, 21, offset);
        		return;
        	}
            DrawQuietZone(graphics, matrix.Width, offset);
            Size paddingOffset = new Size(m_Padding, m_Padding) + new Size(offset.X, offset.Y);
            Size moduleSize = new Size(m_ModuleSize, m_ModuleSize);

            for (int j = 0; j < matrix.Width; j++)
            {
                for (int i = 0; i < matrix.Width; i++)
                {
                    Point moduleRelativePosition = new Point(i * m_ModuleSize, j * m_ModuleSize);
                    Rectangle moduleAbsoluteArea = new Rectangle(moduleRelativePosition + paddingOffset, moduleSize);
                    if(matrix[i, j])
                    	graphics.FillRectangle(m_DarkBrush, moduleAbsoluteArea);
                }
            }
        }

        internal void DrawQuietZone(Graphics graphics, int matrixWidth, Point offset)
        {
        	int barLength = m_ModuleSize * (matrixWidth + (quietZoneModules * 2));
        	graphics.FillRectangle(m_LightBrush, offset.X, offset.Y, barLength, barLength);
        }

        public void CreateImageFile(BitMatrix matrix, string fileName, ImageFormat imageFormat)
        {
            Size size = matrix == null ? Measure(21)
            	: Measure(matrix.Width);
            using (Bitmap bitmap = new Bitmap(size.Width, size.Height))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                Draw(graphics, matrix);
                bitmap.Save(fileName, imageFormat);
            }
        }

        public void WriteToStream(BitMatrix matrix, Stream stream, ImageFormat imageFormat)
        {
            Size size = matrix == null ? Measure(21)
            	: Measure(matrix.Width);
            using (Bitmap bitmap = new Bitmap(size.Width, size.Height))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                Draw(graphics, matrix);
                bitmap.Save(stream, imageFormat);
            }
        }

        public Size Measure(int matrixWidth)
        {
            int areaWidth = m_ModuleSize * matrixWidth;
            m_Padding = quietZoneModules * m_ModuleSize;
            int padding = m_Padding;
            int totalWidth = areaWidth + 2 * padding;
            return new Size(totalWidth + 1, totalWidth + 1);
        }
        
        public QuietZoneModules QuietZoneModules
        {
        	get
        	{
        		return (QuietZoneModules)quietZoneModules;
        	}
        	set
        	{
        		quietZoneModules = (int)value;
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
        		if(value > 0)
        		{
        			m_ModuleSize = value;
        		}
        		else
        			throw new System.ArgumentOutOfRangeException("ModuleSize must be bigger than Zero");
        	}
        }
        
        public Brush DarkBrush
        {
        	get
        	{
        		return m_DarkBrush;
        	}
        	set
        	{
        		m_DarkBrush = value;
        	}
        }
        
        public Brush LightBrush
        {
        	get
        	{
        		return m_LightBrush;
        	}
        	set
        	{
        		m_LightBrush = value;
        	}
        }
        
    }
}
