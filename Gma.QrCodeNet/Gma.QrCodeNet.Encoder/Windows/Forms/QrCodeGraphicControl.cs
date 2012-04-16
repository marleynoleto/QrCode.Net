using System;
using System.Windows.Forms;
using System.Drawing;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.ComponentModel;

namespace Gma.QrCodeNet.Encoding.Windows.Forms
{
    public class QrCodeGraphicControl : Control
    {
        private QrCode m_QrCode = new QrCode();

        private bool m_isLocked = false;
        private bool m_isFreezed = false;

        public event EventHandler DarkBrushChanged;
        public event EventHandler LightBrushChanged;
        public event EventHandler QuietZoneModuleChanged;
        public event EventHandler ErrorCorrectLevelChanged;
        public event EventHandler QrMatrixChanged;

        #region DarkBrush
        private Brush m_darkBrush = Brushes.Black;
        [Category("Qr Code"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(false)]
        public Brush DarkBrush
        {
            get
            {
                return m_darkBrush;
            }
            set
            {
                if (m_darkBrush != value)
                {
                    m_darkBrush = value;
                    OnDarkBrushChanged(new EventArgs());
                    if (!m_isFreezed)
                        this.Invalidate();
                }
            }
        }

        protected virtual void OnDarkBrushChanged(EventArgs e)
        {
            if(DarkBrushChanged != null)
            {
                DarkBrushChanged(this, e);
            }
        }

        #endregion

        #region LightBrush
        private Brush m_LightBrush = Brushes.White;
        [Category("Qr Code"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(false)]
        public Brush LightBrush
        {
            get
            { return m_LightBrush; }
            set
            {
                if (m_LightBrush != value)
                {
                    m_LightBrush = value;
                    OnLightBrushChanged(new EventArgs());
                    if (!m_isFreezed)
                        this.Invalidate();
                }
            }
        }

        protected virtual void OnLightBrushChanged(EventArgs e)
        {
            if (LightBrushChanged != null)
                LightBrushChanged(this, e);
        }

        #endregion

        #region QuietZoneModule
        private QuietZoneModules m_QuietZoneModule = QuietZoneModules.Two;
        [Category("Qr Code"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(false)]
        public QuietZoneModules QuietZoneModule
        {
            get { return m_QuietZoneModule; }
            set
            {
                if (m_QuietZoneModule != value)
                {
                    m_QuietZoneModule = value;
                    OnQuietZoneModuleChanged(new EventArgs());
                    if(!m_isFreezed)
                        this.Invalidate();
                }
            }
        }

        protected virtual void OnQuietZoneModuleChanged(EventArgs e)
        {
            if (QuietZoneModuleChanged != null)
                QuietZoneModuleChanged(this, e);
        }
        #endregion

        #region ErrorCorrectLevel
        private ErrorCorrectionLevel m_ErrorCorrectLevel = ErrorCorrectionLevel.M;
        [Category("Qr Code"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always), RefreshProperties(RefreshProperties.All), Localizable(false)]
        public ErrorCorrectionLevel ErrorCorrectLevel
        {
            get { return m_ErrorCorrectLevel; }
            set
            {
                if (m_ErrorCorrectLevel != value)
                {
                    m_ErrorCorrectLevel = value;

                    this.UpdateQrCodeCache();
                    OnErrorCorrectLevelChanged(new EventArgs());
                }
            }
        }

        protected virtual void OnErrorCorrectLevelChanged(EventArgs e)
        {
            if (ErrorCorrectLevelChanged != null)
                ErrorCorrectLevelChanged(this, e);
        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            
            int offsetX, offsetY, width;
            if (this.Width <= this.Height)
            {
                offsetX = 0;
                offsetY = (this.Height - this.Width) / 2;
                width = this.Width;
            }
            else
            {
                offsetX = (this.Width - this.Height) / 2;
                offsetY = 0;
                width = this.Height;
            }

            new GraphicsRenderer(new FixedCodeSize(width, m_QuietZoneModule), m_darkBrush, m_LightBrush).Draw(e.Graphics, m_QrCode.Matrix, new Point(offsetX, offsetY));

            base.OnPaint(e);
        }

        public void UpdateQrCodeCache()
        {
            if (!m_isLocked)
            {
                new QrEncoder(m_ErrorCorrectLevel).TryEncode(Text, out m_QrCode);
                OnQrMatrixChanged(new EventArgs());
                this.Invalidate();
            }
        }

        protected virtual void OnQrMatrixChanged(EventArgs e)
        {
            if (QrMatrixChanged != null)
                QrMatrixChanged(this, e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            this.UpdateQrCodeCache();
            base.OnTextChanged(e);
        }

        /// <summary>
        /// Lock Class, that any change to Text or ErrorCorrectLevel won't cause it to update QrCode Matrix
        /// </summary>
        public void Lock()
        { m_isLocked = true; }

        /// <summary>
        /// Unlock Class, then update QrCodeMatrix and repaint
        /// </summary>
        public void UnLock()
        { 
            m_isLocked = false;
            UpdateQrCodeCache();
        }

        /// <summary>
        /// Freeze Class, Any value change to Brush, QuietZoneModule won't cause immediately repaint. 
        /// </summary>
        /// <remarks>It won't stop any repaint cause by other action.</remarks>
        public void Freeze()
        { m_isFreezed = true; }

        /// <summary>
        /// Unfreeze and repaint immediately. 
        /// </summary>
        public void UnFreeze()
        { 
            m_isFreezed = false;
            Invalidate();
        }

        /// <summary>
        /// Return whether if class is freezed. 
        /// </summary>
        public bool IsFreezed
        { get { return m_isFreezed; } }

        /// <summary>
        /// Return whether if class is locked
        /// </summary>
        public bool IsLocked
        { get { return m_isLocked; } }

        /// <summary>
        /// Get Qr BitMatrix as two dimentional bool array.
        /// </summary>
        /// <returns>null if matrix is null, else full matrix</returns>
        public bool[,] GetQrMatrix()
        {
            if (m_QrCode.Matrix == null)
                return null;
            else
            {
                bool[,] clone = new bool[m_QrCode.Matrix.Width, m_QrCode.Matrix.Width];
                BitMatrix matrix = m_QrCode.Matrix;
                for (int x = 0; x < matrix.Width; x++)
                {
                    for (int y = 0; y < matrix.Width; y++)
                    {
                        clone[x, y] = matrix[x, y];
                    }
                }
                return clone;
            }
        }

    }
}
