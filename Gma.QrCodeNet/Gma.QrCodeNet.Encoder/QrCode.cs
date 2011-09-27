using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding
{
    public class QrCode
    {
        internal QrCode(QRCodeInternal qrCodeInternal)
        {
            this.Matrix = new SimpleBitMatrix(qrCodeInternal.Matrix);
        }

        public BitMatrix Matrix
        {
            get;
            private set;
        }
    }
}