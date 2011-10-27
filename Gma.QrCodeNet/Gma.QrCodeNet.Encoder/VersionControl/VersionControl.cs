using Mode = Gma.QrCodeNet.Encoding.DataEncodation.Mode;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.DataEncodation;

namespace Gma.QrCodeNet.Encoding.VersionControl
{
	internal static class VersionControl
	{
		private const int NumBitsModeIndicator = 4;
		private const int NumBitsCharCountMax = 16;
		private const string DefaultEncoding = "iso-8859-1";
		
		private static VersionTable versionTable = new VersionTable();
		
		
		
		internal static QRCodeBox InitialSetup(int dataBitsLength, Mode mode, ErrorCorrectionLevel level)
		{
			string encodingName = "";
			
			encodingName = mode == Mode.EightBitByte ? DefaultEncoding : encodingName;
			
			return InitialSetup(dataBitsLength, mode, level, encodingName);
		}
		
		
		internal static QRCodeBox InitialSetup(int dataBitsLength,  Mode mode, ErrorCorrectionLevel level, string encodingName)
		{
			int totalDataBits = dataBitsLength + NumBitsModeIndicator + NumBitsCharCountMax;
			
			bool containECI = false;
			
			if(mode == Mode.EightBitByte)
			{
				if(encodingName != DefaultEncoding)
				{
					totalDataBits += NumBitsModeIndicator;
				
					int eciValue = ECISet.GetECIValueByName(encodingName);
				
					totalDataBits += ECISet.NumOfECIHeaderBits(eciValue);
					
					containECI = true;
				}
			}
			
			int totalDataBytes = totalDataBits / 8;
			totalDataBytes = totalDataBits % 8 == 0 ? totalDataBytes : totalDataBytes + 1;
			
			QRCodeBox qrCodeBox = new QRCodeBox();
			
			qrCodeBox.Version = BinarySearch(totalDataBytes, level, 1, 40);
			
			Version versionData = versionTable.GetVersionByNum(qrCodeBox.Version);
			qrCodeBox.Mode = mode;
			qrCodeBox.ErrorCorrectionLevel = level;
			qrCodeBox.MatrixWidth = versionData.DimensionForVersion;
			qrCodeBox.NumTotalBytes = versionData.TotalCodewords;
			ErrorCorrectionBlocks ecBlocks = versionData.GetECBlocksByLevel(level);
			qrCodeBox.NumErrorCorrectionBytes = ecBlocks.NumErrorCorrectionCodewards;
			qrCodeBox.NumDataBytes = qrCodeBox.NumTotalBytes - qrCodeBox.NumErrorCorrectionBytes;
			qrCodeBox.NumErrorCorrectionBlocks = ecBlocks.NumBlocks;
			
			if(containECI)
			{
				qrCodeBox.ECIDataBits = ECISet.GetECIHeader(encodingName);
			}
			
			return qrCodeBox;
			
		}
		
		private static int BinarySearch(int NumDataCodewords, ErrorCorrectionLevel level, int low, int high)
		{
			int middle;
			
			if(low > high)
				return low;
			
			middle = (low + high) / 2;
			Version version = versionTable.GetVersionByNum(middle);
			int totalCodewords = version.TotalCodewords;
			int numECCodewords = version.GetECBlocksByLevel(level).NumErrorCorrectionCodewards;
			
			int dataCodewords = totalCodewords - numECCodewords;
			
			if(dataCodewords == NumDataCodewords)
				return middle;
			
			if(dataCodewords > NumDataCodewords)
				return BinarySearch(NumDataCodewords, level, low, middle - 1);
			else
				return BinarySearch(NumDataCodewords, level, middle + 1, high);
			
		}
		
	}
}
