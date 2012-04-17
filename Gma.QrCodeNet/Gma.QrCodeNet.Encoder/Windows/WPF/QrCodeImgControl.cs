using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace Gma.QrCodeNet.Encoding.Windows.WPF
{
    public class QrCodeImgControl : Control
    {
        private QrCode m_QrCode = new QrCode();
        private bool m_isLocked = false;
        private bool m_isFreezed = false;

        public event EventHandler QrMatrixChanged;

        #region WBitmap
        public static readonly DependencyProperty WBitmapProperty =
            DependencyProperty.Register("WBitmap",
            typeof(WriteableBitmap),
            typeof(QrCodeImgControl),
            new UIPropertyMetadata(null, null));

        public WriteableBitmap WBitmap
        {
            get { return (WriteableBitmap)GetValue(WBitmapProperty); }
            private set { SetValue(WBitmapProperty, value); }
        }
        #endregion

        #region QrCodeWidth
        public static readonly DependencyProperty QrCodeWidthProperty =
            DependencyProperty.Register("QrCodeWidth", typeof(int), typeof(QrCodeImgControl),
            new UIPropertyMetadata(200, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public int QrCodeWidth
        {
            get { return (int)GetValue(QrCodeWidthProperty); }
            set { SetValue(QrCodeWidthProperty, value); }
        }

        #endregion

        #region QrCodeHeight
        public static readonly DependencyProperty QrCodeHeightProperty =
            DependencyProperty.Register("QrCodeHeight", typeof(int), typeof(QrCodeImgControl),
            new UIPropertyMetadata(200, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public int QrCodeHeight
        {
            get { return (int)GetValue(QrCodeHeightProperty); }
            set { SetValue(QrCodeHeightProperty, value); }
        }

        #endregion

        #region QuietZoneModule
        public static readonly DependencyProperty QuietZoneModuleProperty =
            DependencyProperty.Register("QuietZoneModule", typeof(QuietZoneModules), typeof(QrCodeImgControl),
            new UIPropertyMetadata(QuietZoneModules.Two, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public QuietZoneModules QuietZoneModule
        {
            get { return (QuietZoneModules)GetValue(QuietZoneModuleProperty); }
            set { SetValue(QuietZoneModuleProperty, value); }
        }

        #endregion

        #region LightColor
        public static readonly DependencyProperty LightColorProperty =
            DependencyProperty.Register("LightColor", typeof(Color), typeof(QrCodeImgControl),
            new UIPropertyMetadata(Colors.White, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public Color LightColor
        {
            get { return (Color)GetValue(LightColorProperty); }
            set { SetValue(LightColorProperty, value); }
        }

        #endregion

        #region DarkColor
        public static readonly DependencyProperty DarkColorProperty =
            DependencyProperty.Register("DarkColor", typeof(Color), typeof(QrCodeImgControl),
            new UIPropertyMetadata(Colors.Black, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public Color DarkColor
        {
            get { return (Color)GetValue(DarkColorProperty); }
            set { SetValue(DarkColorProperty, value); }
        }

        #endregion

        #region ErrorCorrectionLevel
        public static readonly DependencyProperty ErrorCorrectLevelProperty =
            DependencyProperty.Register("ErrorCorrectLevel", typeof(ErrorCorrectionLevel), typeof(QrCodeImgControl),
            new UIPropertyMetadata(ErrorCorrectionLevel.M, new PropertyChangedCallback(OnQrValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public ErrorCorrectionLevel ErrorCorrectLevel
        {
            get { return (ErrorCorrectionLevel)GetValue(ErrorCorrectLevelProperty); }
            set { SetValue(ErrorCorrectLevelProperty, value); }
        }
        #endregion

        #region Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(QrCodeImgControl),
            new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(OnQrValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion

        #region IsGrayImage
        public static readonly DependencyProperty IsGrayImageProperty =
            DependencyProperty.Register("IsGrayImage", typeof(bool), typeof(QrCodeImgControl),
            new UIPropertyMetadata(true, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public bool IsGrayImage
        {
            get { return (bool)GetValue(IsGrayImageProperty); }
            set { SetValue(IsGrayImageProperty, value); }
        }

        #endregion

        #region DpiX
        public static readonly DependencyProperty DpiXProperty =
            DependencyProperty.Register("DpiX", typeof(int), typeof(QrCodeImgControl),
            new UIPropertyMetadata(96, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public int DpiX
        {
            get { return (int)GetValue(DpiXProperty); }
            set { SetValue(DpiXProperty, value); }
        }

        #endregion

        #region DpiY
        public static readonly DependencyProperty DpiYProperty =
            DependencyProperty.Register("DpiY", typeof(int), typeof(QrCodeImgControl),
            new UIPropertyMetadata(96, new PropertyChangedCallback(OnVisualValueChanged)));

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Category("QrCode")]
        public int DpiY
        {
            get { return (int)GetValue(DpiYProperty); }
            set { SetValue(DpiYProperty, value); }
        }

        #endregion

        static QrCodeImgControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QrCodeImgControl), new FrameworkPropertyMetadata(typeof(QrCodeImgControl)));
            HorizontalAlignmentProperty.OverrideMetadata(typeof(QrCodeImgControl), new FrameworkPropertyMetadata(HorizontalAlignment.Center));
            VerticalAlignmentProperty.OverrideMetadata(typeof(QrCodeImgControl), new FrameworkPropertyMetadata(VerticalAlignment.Center));
        }

        public QrCodeImgControl()
        {
            this.EncodeAndUpdateBitmap();
        }

        #region ReDraw Bitmap, Update Qr Cache

        private void CreateBitmap()
        {
            WBitmap = null;

            int suitableWidth = m_QrCode.Matrix == null ? CalculateSuitableWidth(QrCodeWidth, 21) 
                : CalculateSuitableWidth(QrCodeWidth, m_QrCode.Matrix.Width);
            
            if (IsGrayImage)
                WBitmap = new WriteableBitmap(suitableWidth, suitableWidth, DpiX, DpiY, PixelFormats.Gray8, null);
            else
                WBitmap = new WriteableBitmap(suitableWidth, suitableWidth, DpiX, DpiY, PixelFormats.Pbgra32, null);
        }

        private int CalculateSuitableWidth(int width, int bitMatrixWidth)
        {
            FixedCodeSize isize = new FixedCodeSize(width, QuietZoneModule);
            DrawingSize dSize = isize.GetSize(bitMatrixWidth);
            int gap = dSize.CodeWidth - dSize.ModuleSize * (bitMatrixWidth + 2 * (int)QuietZoneModule);

            if (gap == 0)
                return width;
            else if (dSize.CodeWidth / gap < 6)
                return (dSize.ModuleSize + 1) * (bitMatrixWidth + 2 * (int)QuietZoneModule);
            else
                return dSize.ModuleSize * (bitMatrixWidth + 2 * (int)QuietZoneModule);
        }

        private void UpdateSource()
        {
            this.CreateBitmap();

            if (QrCodeWidth != 0 && QrCodeHeight != 0)
                WBitmap.Clear(LightColor);

            if (m_QrCode.Matrix != null)
            {
                //WBitmap.
                new WriteableBitmapRenderer(new FixedCodeSize(WBitmap.PixelWidth, QuietZoneModule), DarkColor, LightColor).DrawDarkModule(WBitmap, m_QrCode.Matrix, 0, 0);
            }
        }

        private void UpdateQrCodeCache()
        {
            new QrEncoder(ErrorCorrectLevel).TryEncode(this.Text, out m_QrCode);
            OnQrMatrixChanged(new EventArgs());
        }

        #endregion

        #region Event method

        public static void OnVisualValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((QrCodeImgControl)d).UpdateBitmap();
        }

        /// <summary>
        /// Encode and Update bitmap when ErrorCorrectlevel or Text changed. 
        /// </summary>
        public static void OnQrValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((QrCodeImgControl)d).EncodeAndUpdateBitmap();
        }

        #endregion

        #region Update method

        public void EncodeAndUpdateBitmap()
        {
            if (!IsLocked)
            {
                this.UpdateQrCodeCache();
                this.UpdateBitmap();
            }
        }

        public void UpdateBitmap()
        {
            if(!IsFreezed)
                this.UpdateSource();
        }

        #endregion

        #region Lock Freeze

        /// <summary>
        /// If Class is locked, it won't update QrMatrix cache.
        /// </summary>
        public void Lock()
        {
            m_isLocked = true;
        }

        /// <summary>
        /// Unlock class will cause class to update QrMatrix Cache and redraw bitmap. 
        /// </summary>
        public void Unlock()
        {
            m_isLocked = false;
            this.EncodeAndUpdateBitmap();
        }

        /// <summary>
        /// Return whether if class is locked
        /// </summary>
        public bool IsLocked
        { get { return m_isLocked; } }

        /// <summary>
        /// Freeze Class, Any value change to Brush, QuietZoneModule won't cause immediately redraw bitmap. 
        /// </summary>
        public void Freeze()
        { m_isFreezed = true; }

        /// <summary>
        /// Unfreeze and redraw immediately. 
        /// </summary>
        public void UnFreeze()
        {
            m_isFreezed = false;
            this.UpdateBitmap();
        }

        /// <summary>
        /// Return whether if class is freezed. 
        /// </summary>
        public bool IsFreezed
        { get { return m_isFreezed; } }

        #endregion

        /// <summary>
        /// QrCode matrix cache updated.
        /// </summary>
        protected virtual void OnQrMatrixChanged(EventArgs e)
        {
            if (QrMatrixChanged != null)
                QrMatrixChanged(this, e);
        }

        /// <summary>
        /// Get Qr BitMatrix as two dimentional bool array.
        /// </summary>
        /// <returns>null if matrix is null, else full matrix</returns>
        public BitMatrix GetQrMatrix()
        {
            if (m_QrCode.Matrix == null)
                return null;
            else
            {
                BitMatrix matrix = m_QrCode.Matrix;
                TriStateMatrix clone = new TriStateMatrix(matrix.Width);
                for (int x = 0; x < matrix.Width; x++)
                {
                    for (int y = 0; y < matrix.Width; y++)
                    {
                        clone[x, y, MatrixStatus.NoMask] = matrix[x, y];
                    }
                }
                return clone;
            }
        }

    }
}
