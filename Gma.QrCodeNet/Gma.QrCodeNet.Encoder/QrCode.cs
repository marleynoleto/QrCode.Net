using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding
{
    public class QrCode
    {
        internal QrCode(QRCodeInternal qrCodeInternal)
        {
            SimpleBitMatrix simpleBitMatrix = new SimpleBitMatrix(qrCodeInternal.MatrixWidth);
            for (int i = 0; i < qrCodeInternal.MatrixWidth; i++)
            {
                for (int j = 0; j < qrCodeInternal.MatrixWidth; j++)
                {
                    simpleBitMatrix.Set(i, j , (qrCodeInternal.Matrix[j, i] != 0));
                }
            }
            this.Matrix = simpleBitMatrix;
        }

        public BitMatrix Matrix
        {
            get;
            private set;
        }
    }
}