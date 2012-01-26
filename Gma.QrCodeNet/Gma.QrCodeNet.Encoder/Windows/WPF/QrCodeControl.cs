using System;
using System.IO;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace Gma.QrCodeNet.Encoding.Windows.WPF
{
    public class QrCodeControl : Canvas
    {
    	private readonly QrEncoder m_Encoder;
        private readonly Renderer m_Renderer;
        private QrCode m_QrCode;

        private Color m_DarkBrushColor = Colors.Black;
        private Color m_LightBrushColor = Colors.White;


        /// <summary>
        /// QrCode WPF control base on Canvas. Use DrawingContext
        /// Low memory, No stretch mode. 
        /// </summary>
        public QrCodeControl()
            : this(new QrEncoder(ErrorCorrectionLevel.H), new Renderer(7))
        {

        }


        /// <summary>
        /// QrCode WPF control base on Canvas. Use DrawingContext
        /// Low memory, No stretch mode. 
        /// </summary>
        /// <param name="encoder">QrEncoder, Specify errorcorrection level</param>
        /// <param name="renderer">Renderer, Specify module size</param>
        public QrCodeControl(QrEncoder encoder, Renderer renderer)
        {
            m_Encoder = encoder;
            m_Renderer = renderer;
            m_QrCode = new QrCode();
            this.Initialize();
        }

        private void Initialize()
        {
            m_IMG_Height = m_Renderer.Measure(21);
            m_IMG_Width = m_IMG_Height;
            this.SnapsToDevicePixels = true;
            this.VisualBitmapScalingMode = BitmapScalingMode.HighQuality;
        }
        
        private double m_IMG_Height = 0;
        private double m_IMG_Width = 0;
        

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            m_Renderer.Draw(dc, m_QrCode.Matrix);
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
        
        private bool m_AutoSize = true;
        
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
        			this.AdjustSize();
                    this.InvalidateVisual();
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
        		
        			this.AdjustSize();
                    this.InvalidateVisual();
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
        		
        			this.AdjustSize();
                    this.InvalidateVisual();
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
        			this.AdjustSize();
                    this.InvalidateVisual();
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
                    this.InvalidateVisual();
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
                    this.InvalidateVisual();
                }
            }
        }
        
    }
}
