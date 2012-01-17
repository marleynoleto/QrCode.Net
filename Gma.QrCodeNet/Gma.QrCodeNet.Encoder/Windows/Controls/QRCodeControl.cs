using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using Gma.QrCodeNet.Encoding;

namespace Gma.QrCodeNet.Encoding.Windows.Controls
{
    [ToolboxBitmap(typeof(QrCodeControl), "QrCodeControlIcon")]
    public class QrCodeControl : Control
    {
        private readonly QrEncoder m_Encoder;
        private readonly Renderer m_Renderer;
        
        private Color m_DarkBrushColor = Color.Black;
        private Color m_LightBrushColor = Color.White;
        
        private QrCode m_QrCode;

        public QrCodeControl()
            : this(new QrEncoder(ErrorCorrectionLevel.H), new Renderer(7))
        {

        }

        public QrCodeControl(QrEncoder encoder, Renderer renderer)
        {
            m_Encoder = encoder;
            m_Renderer = renderer;
            m_QrCode = new QrCode();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
        	if(!string.IsNullOrEmpty(this.Text) && m_QrCode.isContainMatrix)
        	{
            	m_Renderer.Draw(e.Graphics, m_QrCode.Matrix);
            	if (Artistic)
            	{
                	DrawText(e.Graphics);
            	}
        	}
        	else
        	{
        		m_Renderer.DrawQuietZone(e.Graphics, 21, new Point(0,0));
        	}
            base.OnPaint(e);
        }

        private void DrawText(Graphics graphics)
        {
        	if (string.IsNullOrEmpty(this.Text) || this.Text.Trim() == string.Empty)
            {
                return;
            }

            float diseredTextHeight = (float)(this.Size.Height / 6.0);
            float padding = (float)(diseredTextHeight*0.05);
            SizeF availableAreaSize = new SizeF(this.Size.Width-padding, diseredTextHeight-padding);
            float fontSize = CalculateFontSize(graphics, availableAreaSize, this.Text, this.Font);
            Font font = new Font(this.Font.FontFamily, fontSize, this.Font.Style);
            SizeF textSize = TextRenderer.MeasureText(graphics, this.Text, font, new Size(0,0));

            Color semiTransparentWhite = Color.FromArgb(240, Color.White);
            float yPositionRectangle = (this.Size.Height - availableAreaSize.Height) / 2;
            RectangleF semiTransparentArea = new RectangleF(new PointF(0, yPositionRectangle), availableAreaSize);
            graphics.FillRectangle(new SolidBrush(semiTransparentWhite), semiTransparentArea);

            float yPosition = (this.Size.Height - textSize.Height + padding) / 2;
            float xPosition = this.Size.Width;

            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            TextRenderer.DrawText(graphics, this.Text, font, new Point((int) xPosition, (int) yPosition), Color.Black, TextFormatFlags.HorizontalCenter);
        }

        public static float CalculateFontSize(Graphics graphics, SizeF availableArea, string text, Font prototype)
        {
            SizeF realSize = TextRenderer.MeasureText(graphics, text, prototype);

            float hRatio = availableArea.Height / realSize.Height;
            float wRatio = availableArea.Width / realSize.Width;
            float ratio = (hRatio < wRatio) ? hRatio : wRatio;

            float newSize = prototype.Size * ratio;

            return newSize;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            this.UpdateQrCodeCache();
            base.OnTextChanged(e);
            this.UpdatePaint();
        }

        private void UpdateQrCodeCache()
        {
            m_Encoder.TryEncode(this.Text, out m_QrCode);
        }
        
        private void UpdatePaint()
        {
        	if (this.AutoSize)
            {
                this.AdjustSize();
            }
            this.Invalidate();
        }

        private bool m_Artistic;

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(true),
         DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("QR Code")]
        public bool Artistic
        {
            get { return m_Artistic; }
            set
            {
            	if(m_Artistic != value)
            	{
            		m_Artistic = m_Encoder.ErrorCorrectionLevel != ErrorCorrectionLevel.H ? false
                		: value;
            		if(m_Artistic == value)
            			this.UpdatePaint();
            	}
            }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("Layout"), RefreshProperties(RefreshProperties.All), Localizable(false), Description("Auto size"), DefaultValue(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
            	if(base.AutoSize != value)
            	{
            		base.AutoSize = value;
            		if(value)
            			this.UpdatePaint();
            	}
            }
        }


        public override Size GetPreferredSize(Size proposedSize)
        {
            return m_QrCode.isContainMatrix ? m_Renderer.Measure(m_QrCode.Matrix.Width)
            	: m_Renderer.Measure(21);	//21 is width for version 01. 
        }


        internal void AdjustSize()
        {
            if (AnchorsRestrictSize())
            {
                return;
            }

            this.Size = this.PreferredSize;
        }

        private bool AnchorsRestrictSize()
        {
            return (((this.Anchor & (AnchorStyles.Right | AnchorStyles.Left)) == (AnchorStyles.Right | AnchorStyles.Left)) ||
                    ((this.Anchor & (AnchorStyles.Bottom | AnchorStyles.Top)) == (AnchorStyles.Bottom | AnchorStyles.Top)));
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
        		
        			this.UpdatePaint();
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
        		
        			this.UpdatePaint();
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
        		
        			Artistic = value != ErrorCorrectionLevel.H ? false
        				: m_Artistic;
        		
        			this.UpdateQrCodeCache();
        			this.UpdatePaint();
        		}
             }
        }
        
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(false),
         DefaultValue(ErrorCorrectionLevel.H), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("QR Code")]
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
					this.UpdatePaint();
				}
			}
		}
		
		[Browsable(true), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(false),
         DefaultValue(ErrorCorrectionLevel.H), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("QR Code")]
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
					this.UpdatePaint();
				}
			}
		}
        
        
    }
}
