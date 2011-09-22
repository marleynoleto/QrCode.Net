using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding
{
    public class QrCode
    {
        internal QrCode(QRCodeInternal qrCodeInternal)
        {
            Matrix = new BitMatrix(qrCodeInternal.MatrixWidth);
            for (int i = 0; i < qrCodeInternal.MatrixWidth; i++)
            {
                for (int j = 0; j < qrCodeInternal.MatrixWidth; j++)
                {
                    Matrix[i, j] = (qrCodeInternal.Matrix.get_Renamed(i, j) != 0);
                }
            }
        }

        public BitMatrix Matrix
        {
            get;
            private set;
        }
    }
}