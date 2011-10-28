using Gma.QrCodeNet.Encoding.DataEncodation;

namespace Gma.QrCodeNet.Encoding.Versions
{
	public static class VersionControl
	{
		private const int NUM_BITS_MODE_INDICATOR = 4;
		private const string DEFAULT_ENCODING = "iso-8859-1";
		
		private static VersionTable versionTable = new VersionTable();
		
		
		/// <summary>
		/// Method use for non-EightBitByte encoding, or default EightBitByte encoding
		/// Default Encoding = iso-8859-1
		/// </summary>
		/// <param name="dataBitsLength">Number of bits for encoded content</param>
		public static QRCodeBox InitialSetup(int dataBitsLength, Mode mode, ErrorCorrectionLevel level)
		{
			string encodingName = "";
			
			encodingName = mode == Mode.EightBitByte ? DEFAULT_ENCODING : encodingName;
			
			return InitialSetup(dataBitsLength, mode, level, encodingName);
		}
		
		/// <param name="dataBitsLength">Number of bits for encoded content</param>
		/// <param name="encodingName">Encoding name for EightBitByte</param>
		public static QRCodeBox InitialSetup(int dataBitsLength,  Mode mode, ErrorCorrectionLevel level, string encodingName)
		{
			int totalDataBits = dataBitsLength;
			
			bool containECI = false;
			
			if(mode == Mode.EightBitByte)
			{
				if(encodingName != DEFAULT_ENCODING)
				{
					int eciValue = ECISet.GetECIValueByName(encodingName);
				
					totalDataBits += ECISet.NumOfECIHeaderBits(eciValue);
					
					containECI = true;
				}
			}
			int searchGroup = DynamicSearchIndicator(totalDataBits, level, mode);
			
			int[] charCountIndicator = CharCountIndicatorTable.GetCharCountIndicator(mode);
			
			totalDataBits += (NUM_BITS_MODE_INDICATOR + charCountIndicator[searchGroup]);
			
			int lowerSearchBoundary = searchGroup == 0 ? 1 : (VERSION_GROUP[searchGroup - 1] + 1);
			int higherSearchBoundary = VERSION_GROUP[searchGroup];
			
			
			int versionNum = BinarySearch(totalDataBits, level, lowerSearchBoundary, higherSearchBoundary);
			
			return FillCodeBox(versionNum, mode, level, encodingName, containECI);
			
		}
		
		
//		private static int ToNumOfBytes(int numOfBits)
//		{
//			return numOfBits % 8 == 0 ? numOfBits / 8
//				: (numOfBits / 8) + 1;
//		}
//		
		
		private static QRCodeBox FillCodeBox(int versionNum, Mode mode, ErrorCorrectionLevel level, string encodingName, bool isContainECI)
		{
			if(versionNum < 1 || versionNum > 40)
			{
				throw new System.InvalidOperationException(string.Format("Unexpected version number: {0}", versionNum));
			}
			
			QRCodeBox qrCodeBox = new QRCodeBox();
			
			qrCodeBox.Version = versionNum;
			
			Version versionData = versionTable.GetVersionByNum(qrCodeBox.Version);
			qrCodeBox.Mode = mode;
			qrCodeBox.ErrorCorrectionLevel = level;
			qrCodeBox.MatrixWidth = versionData.DimensionForVersion;
			qrCodeBox.NumTotalBytes = versionData.TotalCodewords;
			
			ErrorCorrectionBlocks ecBlocks = versionData.GetECBlocksByLevel(level);
			qrCodeBox.NumErrorCorrectionBytes = ecBlocks.NumErrorCorrectionCodewards;
			qrCodeBox.NumDataBytes = qrCodeBox.NumTotalBytes - qrCodeBox.NumErrorCorrectionBytes;
			qrCodeBox.NumErrorCorrectionBlocks = ecBlocks.NumBlocks;
			
			if(isContainECI)
			{
				qrCodeBox.ECIDataBits = ECISet.GetECIHeader(encodingName);
			}
			
			return qrCodeBox;
		}
		
		
		private static readonly int[] VERSION_GROUP = new int[]{9, 26, 40};
		
		private static int DynamicSearchIndicator(int numBits, ErrorCorrectionLevel level, Mode mode)
		{
			int[] charCountIndicator = CharCountIndicatorTable.GetCharCountIndicator(mode);
			int totalBits = 0;
			int loopLength = VERSION_GROUP.Length;
			for(int i = 0; i < loopLength; i++)
			{
				totalBits = numBits + NUM_BITS_MODE_INDICATOR + charCountIndicator[i];
				
				Version version = versionTable.GetVersionByNum(VERSION_GROUP[i]);
				int totalCodewords = version.TotalCodewords;
				int numECCodewords = version.GetECBlocksByLevel(level).NumErrorCorrectionCodewards;
			
				int dataCodewords = totalCodewords - numECCodewords;
				
				if(totalBits <= dataCodewords * 8)
				{
					return i;
				}
			}
			
			throw new System.ArgumentOutOfRangeException(string.Format("QRCode do not have enough space for {0} bits"), (numBits + NUM_BITS_MODE_INDICATOR + charCountIndicator[2]).ToString());
			
		}
		
		
		/// <param name="NumDataCodewords">Bytes number</param>
		private static int BinarySearch(int numDataBits, ErrorCorrectionLevel level, int lowerVersionNum, int higherVersionNum)
		{
			int middleVersionNumber;
			
			if(lowerVersionNum > higherVersionNum)
			{
				//Version Low should be the one that has enough space for current data.
				return lowerVersionNum;	
			}
			
			middleVersionNumber = (lowerVersionNum + higherVersionNum) / 2;
			Version version = versionTable.GetVersionByNum(middleVersionNumber);
			int totalCodewords = version.TotalCodewords;
			int numECCodewords = version.GetECBlocksByLevel(level).NumErrorCorrectionCodewards;
			
			int dataCodewords = totalCodewords - numECCodewords;
			
			if(dataCodewords * 8 == numDataBits)
				return middleVersionNumber;
			
			if(dataCodewords * 8 > numDataBits)
				return BinarySearch(numDataBits, level, lowerVersionNum, middleVersionNumber - 1);
			else
				return BinarySearch(numDataBits, level, middleVersionNumber + 1, higherVersionNum);
			
		}
		
	}
}
