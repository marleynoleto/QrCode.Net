using System;
using com.google.zxing.qrcode.decoder;
using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding
{
    public class QrEncoder
    {
        public ErrorCorrectionLevel ErrorCorrectionLevel { get; set; }

        public QrEncoder()
            : this(ErrorCorrectionLevel.M)
        {

        }

        public QrEncoder(ErrorCorrectionLevel errorCorrectionLevel)
        {
            ErrorCorrectionLevel = errorCorrectionLevel;
        }

        public QrCode Encode(string content)
        {
//            ErrorCorrectionLevelInternal level = ErrorCorrectionLevelConverter.ToInternal(this.ErrorCorrectionLevel);
//            QRCodeInternal qrCodeInternal = new QRCodeInternal();
//            EncoderInternal.encode(content, level, qrCodeInternal);
			return string.IsNullOrEmpty(content) ? new QrCode(QRCodeEncode.Encode(" ", ErrorCorrectionLevel))
				: new QrCode(QRCodeEncode.Encode(content, ErrorCorrectionLevel));
        }
    }
}
