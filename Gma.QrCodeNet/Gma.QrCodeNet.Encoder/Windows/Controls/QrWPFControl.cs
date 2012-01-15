using System;
using System.IO;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace Gma.QrCodeNet.Encoding.Windows.Controls
{
    public class QrWPFControl : Image
    {
    	private readonly QrEncoder m_Encoder;
        private readonly Renderer m_Renderer;
        private QrCode m_QrCode;

        public QrWPFControl()
            : this(new QrEncoder(ErrorCorrectionLevel.H), new Renderer(7))
        {

        }

        public QrWPFControl(QrEncoder encoder, Renderer renderer)
        {
            m_Encoder = encoder;
            m_Renderer = renderer;
            m_QrCode = new QrCode();
        }
        
        private double m_IMG_Height = 0;
        private double m_IMG_Width = 0;
        
        private void UpdateSource()
        {
        	if(m_QrCode.Matrix == null)
        	{
        		this.Source = null;
        		m_IMG_Height = 0;
        		m_IMG_Width = 0;
        	}
        	else
        	{
        		MemoryStream ms = new MemoryStream();
        		m_Renderer.WriteToStream(m_QrCode.Matrix, ms, ImageFormat.Png);
        		ms.Position = 0;
        		BitmapImage bi = new BitmapImage();
        		bi.BeginInit();
        		bi.StreamSource = ms;
        		bi.EndInit();
        		m_IMG_Height = bi.Height;
        		m_IMG_Width = bi.Width;
        		this.Source = bi;
        	}
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
        
    }
}
