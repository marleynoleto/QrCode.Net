using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.Common;

namespace Gma.QrCodeNet.Encoding
{
    public class QrCode
    {
        internal QrCode(QRCodeInternal qrCodeInternal)
        {
            this.Matrix = qrCodeInternal.Matrix.ToBitMatrix();
        }

        public BitMatrix Matrix
        {
            get;
            private set;
        }
    }
}