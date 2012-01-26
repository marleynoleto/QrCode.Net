using System;
using System.IO;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace Gma.QrCodeNet.Encoding.Windows.WPF
{
    public class QrWPFControl : Image
    {
    	private readonly QrEncoder m_Encoder;
        private readonly Renderer m_Renderer;
        private QrCode m_QrCode;

        private WriteableBitmap m_WriteableBitmap;

        private Color m_DarkBrushColor = Colors.Black;
        private Color m_LightBrushColor = Colors.White;

        /// <summary>
        /// QrCode Control for WPF base on Image control
        /// Memory cost depending on module size inside Renderer. 
        /// Image can be stretch
        /// </summary>
        public QrWPFControl()
            : this(new QrEncoder(ErrorCorrectionLevel.H), new Renderer(3))
        {

        }

        /// <summary>
        /// QrCode Control for WPF base on Image control
        /// Memory cost depending on module size inside Renderer. 
        /// Image can be stretch
        /// </summary>
        /// <param name="encoder">QrEncoder, Specify errorcorrection level</param>
        /// <param name="renderer">Renderer, Specify module size</param>
        public QrWPFControl(QrEncoder encoder, Renderer renderer)
        {
            m_Encoder = encoder;
            m_Renderer = renderer;
            m_QrCode = new QrCode();
            this.Initialize();
        }

        private void Initialize()
        {
            this.UpdateSource();
            this.SnapsToDevicePixels = true;
            this.VisualBitmapScalingMode = BitmapScalingMode.HighQuality;
        }
        
        private double m_IMG_Height = 0;
        private double m_IMG_Width = 0;
        
        private void UpdateSource()
        {
            if (m_WriteableBitmap == null)
            {
                int width = m_Renderer.Measure(21);
                m_WriteableBitmap = new WriteableBitmap(width, width, 96, 96, PixelFormats.Pbgra32, null);

                this.Source = m_WriteableBitmap;
                m_IMG_Height = width;
                m_IMG_Width = width;
            }
            else
            {
                int width = m_QrCode.Matrix == null ? m_Renderer.Measure(21)
                    : m_Renderer.Measure(m_QrCode.Matrix.Width);
                if (width != m_WriteableBitmap.PixelWidth)
                {
                    m_WriteableBitmap = null;
                    m_WriteableBitmap = new WriteableBitmap(width, width, 96, 96, PixelFormats.Pbgra32, null);
                    this.Source = m_WriteableBitmap;
                    m_IMG_Height = width;
                    m_IMG_Width = width;
                }
            }

            m_WriteableBitmap.Clear(m_LightBrushColor);
            if (m_QrCode.Matrix != null)
                m_Renderer.DrawDarkModule(m_WriteableBitmap, m_QrCode.Matrix, new Point(0, 0));
        }

        
        
        private void UpdateQrCodeCache()
        {
        	m_Encoder.TryEncode(m_Text, out m_QrCode);
        }
        
        private void AdjustSize()
        {
        	if(m_AutoSize)
        	{
        		this.Height = m_IMG_Height;
        		this.Width = m_IMG_Width;
        	}
        }
        
        private bool m_AutoSize;
        
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(true),
         DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("QR Code")]
        public bool AutoSize
        {
        	get
        	{
        		return m_AutoSize;
        	}
        	set
        	{
        		m_AutoSize = value;
        		this.AdjustSize();
        	}
        }
        
        private string m_Text;
        
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(true),
         DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("QR Code")]
        public string Text
        {
        	get
        	{
        		return m_Text;
        	}
        	set
        	{
        		if(m_Text != value)
        		{
        			m_Text = value;
        			this.UpdateQrCodeCache();
        			this.UpdateSource();
        			this.AdjustSize();
        		}
        	}
        }
        
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(false),
         DefaultValue(QuietZoneModules.Four), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("QR Code")]
        public QuietZoneModules QuietZoneModules
        {
        	get
        	{
        	 	return m_Renderer.QuietZoneModules;
        	}
        	set
        	{
        		if(m_Renderer.QuietZoneModules != value)
        		{
        			m_Renderer.QuietZoneModules = value;
        		
        			this.UpdateSource();
        			this.AdjustSize();
        		}
        	}
        }
        
       	[Browsable(true), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(false),
         DefaultValue(7), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("QR Code")]
        public int ModuleSize
        {
        	get
        	{
        		return m_Renderer.ModuleSize;
        	}
        	set
        	{
        		if(m_Renderer.ModuleSize != value)
        		{
        			m_Renderer.ModuleSize = value;
        		
        			this.UpdateSource();
        			this.AdjustSize();
        		}
        	}
        }
        
        
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(false),
         DefaultValue(ErrorCorrectionLevel.H), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("QR Code")]
        public ErrorCorrectionLevel ErrorCorrectionLevel
        {
        	get
        	{
        		return m_Encoder.ErrorCorrectionLevel;
        	}
        	set
        	{
        		if(m_Encoder.ErrorCorrectionLevel != value)
        		{
        			m_Encoder.ErrorCorrectionLevel = value;
        		
        			this.UpdateQrCodeCache();
        			this.UpdateSource();
        			this.AdjustSize();
        		}
             }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(false),
         DefaultValue(typeof(Color), "Black"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("QR Code")]
        public Color DarkBrush
        {
            get
            {
                return m_DarkBrushColor;
            }
            set
            {
                if (m_DarkBrushColor != value)
                {
                    m_DarkBrushColor = value;
                    m_Renderer.DarkBrush = m_DarkBrushColor;
                    this.UpdateSource();
                }
            }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(false),
         DefaultValue(typeof(Color), "White"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("QR Code")]
        public Color LightBrush
        {
            get
            {
                return m_LightBrushColor;
            }
            set
            {
                if (m_LightBrushColor != value)
                {
                    m_LightBrushColor = value;
                    m_Renderer.LightBrush = m_LightBrushColor;
                    this.UpdateSource();
                }
            }
        }
        
    }
}
