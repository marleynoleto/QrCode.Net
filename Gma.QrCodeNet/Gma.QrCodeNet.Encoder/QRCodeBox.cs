using com.google.zxing.qrcode.encoder;
using Mode = Gma.QrCodeNet.Encoding.DataEncodation.Mode;

namespace Gma.QrCodeNet.Encoding
{
	internal sealed class QRCodeBox
	{
		private Mode mode;
		
		internal Mode Mode 
		{
			get
			{
				return mode;
			}
			set
			{
				mode = value;
			}
		}
		
		private ErrorCorrectionLevel errorCorrectionLevel;
		
		internal ErrorCorrectionLevel ErrorCorrectionLevel
		{
			get
			{
				return errorCorrectionLevel;
			}
			set
			{
				errorCorrectionLevel = value;
			}
		}
		
		private int version;
		
		internal int Version
		{
			get
			{
				return version;
			}
			set
			{
				version = value;
			}
		}
		
		private int matrixWidth;
		
		
		internal int MatrixWidth
		{
			get
			{
				return matrixWidth;
			}
			set
			{
				matrixWidth = value;
			}
		}
		
		private Masking.MaskPatternType maskPattern;
		
		internal Masking.MaskPatternType MaskPattern
		{
			get
			{
				return maskPattern;
			}
			set
			{
				maskPattern = value;
			}
		}
		
		private int numTotalBytes;
		
		internal int NumTotalBytes
		{
			get
			{
				return numTotalBytes;
			}
			set
			{
				numTotalBytes = value;
			}
		}
		
		private int numDataBytes;
		
		internal int NumDataBytes
		{
			get
			{
				return numDataBytes;
			}
			set
			{
				numDataBytes = value;
			}
		}
		
		
		private int numErrorCorrectionBytes;
		
		internal int NumErrorCorrectionBytes
		{
			get
			{
				return numErrorCorrectionBytes;
			}
			set
			{
				numErrorCorrectionBytes = value;
			}
		}
		
		private int numErrorCorrectionBlocks;
		
		internal int NumErrorCorrectionBlocks
		{
			get
			{
				return numErrorCorrectionBlocks;
			}
			set
			{
				numErrorCorrectionBlocks = value;
			}
		}
		
		private BitVector eciDataBits;
		
		internal BitVector ECIDataBits
		{
			get
			{
				return eciDataBits;
			}
			set
			{
				eciDataBits = value;
			}
		}
		
		private BitVector headerDataBits;
		
		internal BitVector HeaderDataBits
		{
			get
			{
				return headerDataBits;
			}
			set
			{
				headerDataBits = value;
			}
		}
		
	}
}
