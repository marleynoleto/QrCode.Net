using System;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Gma.QrCodeNet.Encoding;

namespace Gma.QrCodeNet.Encoding.Windows.Controls
{
	public class QrCodeImgControl : PictureBox
	{
		private readonly QrEncoder m_Encoder;
        private readonly Renderer m_Renderer;
        
        private Color m_DarkBrushColor = Color.Black;
        private Color m_LightBrushColor = Color.White;
        
        private QrCode m_QrCode;
		
        /// <summary>
        /// QrCode control base on PictureBox. Memory usage may vary by module size and QrCode version. 
        /// It has SizeMode for stretch code image. 
        /// Easier if you want to set SizeMode to PictureBoxSizeMode.Zoom, so QrCode can always fit to constant container. 
        /// </summary>
		public QrCodeImgControl()
			: this(new QrEncoder(ErrorCorrectionLevel.H), new Renderer(7))
		{
		}
		
		/// <summary>
		/// QrCode control base on PictureBox. Memory usage may vary by module size and QrCode version.
		/// It has SizeMode for stretch code image. 
		/// Easier if you want to set SizeMode to PictureBoxSizeMode.Zoom, so QrCode can always fit to constant container. 
		/// </summary>
		public QrCodeImgControl(QrEncoder encoder, Renderer render)
		{
			m_Encoder = encoder;
			m_Renderer = render;
			m_QrCode = new QrCode();
			this.SizeMode = PictureBoxSizeMode.AutoSize;
			UpdateSource();
		}
		
		private void UpdateSource()
        {
			MemoryStream ms = new MemoryStream();
        	m_Renderer.WriteToStream(m_QrCode.Matrix, ms, ImageFormat.Png);
        	Bitmap bitmap = new Bitmap(ms);
        	this.Image = bitmap;
        }
        
        private void UpdateQrCodeCache()
        {
        	m_Encoder.TryEncode(this.Text, out m_QrCode);
        }
        
        protected override void OnTextChanged(EventArgs e)
        {
            this.UpdateQrCodeCache();
            base.OnTextChanged(e);
            this.UpdateSource();
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
				if(m_DarkBrushColor != value)
				{
					m_DarkBrushColor = value;
					m_Renderer.DarkBrush = new SolidBrush(m_DarkBrushColor);
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
				if(m_LightBrushColor != value)
				{
					m_LightBrushColor = value;
					m_Renderer.LightBrush = new SolidBrush(m_LightBrushColor);
					this.UpdateSource();
				}
			}
		}
		

		
		
	}
}
