using System;
using Gma.QrCodeNet.Encoding.DataEncodation;

namespace Gma.QrCodeNet.Encoding.Versions
{
	internal static class VersionControl
	{
		private const int NUM_BITS_MODE_INDICATOR = 4;
		private const string DEFAULT_ENCODING = QRCodeConstantVariable.DefaultEncoding;
		
		private static VersionTable versionTable = new VersionTable();
		
		/// <summary>
		/// Determine which version to use
		/// </summary>
		/// <param name="dataBitsLength">Number of bits for encoded content</param>
		/// <param name="encodingName">Encoding name for EightBitByte</param>
		/// <returns>VersionDetail and ECI</returns>
		internal static VersionControlStruct InitialSetup(int dataBitsLength,  Mode mode, ErrorCorrectionLevel level, string encodingName)
		{
			int totalDataBits = dataBitsLength;
			
			bool containECI = false;
			
			ECISet eciSet = new ECISet(ECISet.AppendOption.NameToValue);
			//Check ECI header
			if(mode == Mode.EightBitByte)
			{
				if(encodingName != DEFAULT_ENCODING)
				{
					int eciValue = eciSet.GetECIValueByName(encodingName);
				
					totalDataBits += ECISet.NumOfECIHeaderBits(eciValue);
					
					containECI = true;
				}
			}
			//Determine which version group it belong to
			int searchGroup = DynamicSearchIndicator(totalDataBits, level, mode);
			
			int[] charCountIndicator = CharCountIndicatorTable.GetCharCountIndicatorSet(mode);
			
			totalDataBits += (NUM_BITS_MODE_INDICATOR + charCountIndicator[searchGroup]);
			
			int lowerSearchBoundary = searchGroup == 0 ? 1 : (VERSION_GROUP[searchGroup - 1] + 1);
			int higherSearchBoundary = VERSION_GROUP[searchGroup];
			
			//Binary search to find proper version
			int versionNum = BinarySearch(totalDataBits, level, lowerSearchBoundary, higherSearchBoundary);
			
			VersionControlStruct vcStruct = FillVCStruct(versionNum, level, encodingName);
			
			vcStruct.isContainECI = containECI;
			
			if(containECI)
			{
				vcStruct.ECIHeader = eciSet.GetECIHeader(encodingName);
			}
			//ZXing decoder doesn't support UTF8's ECI value. 
			//We will not put into QRcode for now. 
			if(encodingName == QRCodeConstantVariable.UTF8Encoding)
				vcStruct.isContainECI = false;
			
			return vcStruct;
			
		}
		
		private static VersionControlStruct FillVCStruct(int versionNum, ErrorCorrectionLevel level, string encodingName)
		{
			if(versionNum < 1 || versionNum > 40)
			{
				throw new InvalidOperationException(string.Format("Unexpected version number: {0}", versionNum));
			}
			
			VersionControlStruct vcStruct = new VersionControlStruct();
			
			int version = versionNum;
			
			QRCodeVersion versionData = versionTable.GetVersionByNum(versionNum);
			
			int numTotalBytes = versionData.TotalCodewords;
			
			ErrorCorrectionBlocks ecBlocks = versionData.GetECBlocksByLevel(level);
			int numDataBytes = numTotalBytes - ecBlocks.NumErrorCorrectionCodewards;
			int numECBlocks = ecBlocks.NumBlocks;
			
			VersionDetail vcDetail = new VersionDetail(version, numTotalBytes, numDataBytes, numECBlocks);
			
			vcStruct.VersionDetail = vcDetail;
			return vcStruct;
		}
		
		
		private static readonly int[] VERSION_GROUP = new int[]{9, 26, 40};
		
		/// <summary>
		/// Decide which version group it belong to
		/// </summary>
		/// <param name="numBits">number of bits for bitlist where it contain DataBits encode from input content and ECI header</param>
		/// <param name="level">Error correction level</param>
		/// <param name="mode">Mode</param>
		/// <returns>Version group index for VERSION_GROUP</returns>
		private static int DynamicSearchIndicator(int numBits, ErrorCorrectionLevel level, Mode mode)
		{
			int[] charCountIndicator = CharCountIndicatorTable.GetCharCountIndicatorSet(mode);
			int totalBits = 0;
			int loopLength = VERSION_GROUP.Length;
			for(int i = 0; i < loopLength; i++)
			{
				totalBits = numBits + NUM_BITS_MODE_INDICATOR + charCountIndicator[i];
				
				QRCodeVersion version = versionTable.GetVersionByNum(VERSION_GROUP[i]);
				int totalCodewords = version.TotalCodewords;
				int numECCodewords = version.GetECBlocksByLevel(level).NumErrorCorrectionCodewards;
			
				int dataCodewords = totalCodewords - numECCodewords;
				
				if(totalBits <= dataCodewords * 8)
				{
					return i;
				}
			}
			
			throw new ArgumentOutOfRangeException(string.Format("QRCode do not have enough space for {0} bits"), (numBits + NUM_BITS_MODE_INDICATOR + charCountIndicator[2]).ToString());
			
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
			QRCodeVersion version = versionTable.GetVersionByNum(middleVersionNumber);
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
