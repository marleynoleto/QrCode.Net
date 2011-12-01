using System;

namespace Gma.QrCodeNet.Encoding.Terminate
{
	internal static class Terminator
	{
		private const int NumBitsForByte = 8;
		
		/// <summary>
		/// This method will create BitList that contain 
		/// terminator, padding and pad codewords for given datacodewords.
		/// Use it to full fill the data codewords capacity. Thus avoid massive empty bits.
		/// </summary>
		/// <remarks>ISO/IEC 18004:2006 P. 32 33. 
		/// Terminator / Bit stream to codeword conversion</remarks>
		/// <param name="dataCodewords">It should include ECI header follow by mode indicator, 
		/// char count indicator, databits.</param>
		/// <param name="numTotalDataCodewords">Total number of datacodewords for specific version.
		/// Receive it under Version/VersionTable</param>
		/// <returns>Bitlist that contain Terminator, padding and padcodewords</returns>
		internal static BitList TerminateBites(BitList dataCodewords, int numTotalDataCodewords)
		{
			int numTotalDataBits = numTotalDataCodewords << 3;
			int numDataBits = dataCodewords.Count;
			
			int numFillerBits = numTotalDataBits - numDataBits;
			int numBitsNeedForLastByte = numFillerBits & 0x7;
			int numFillerBytes = numFillerBits >> 3;
			
			BitList result = new BitList();
			if(numBitsNeedForLastByte >= QRCodeConstantVariable.TerminatorLength)
			{
				result.Add(TerminatorPadding(numBitsNeedForLastByte));
				result.Add(PadeCodewords(numFillerBytes));
			}
			else if(numFillerBytes == 0)
			{
				result.Add(TerminatorPadding(numBitsNeedForLastByte));
			}
			else if(numFillerBytes > 0)
			{
				result.Add(TerminatorPadding(numBitsNeedForLastByte + NumBitsForByte));
				result.Add(PadeCodewords(numFillerBytes - 1));
			}
			
			if(result.Count == numFillerBits)
				return result;
			else
				throw new ArgumentException(
					string.Format("Generate terminator and Padding fail. Num of bits need: {0}, Actually length: {1}", numFillerBytes, result.Count));
		}
		
		
		private static BitList PadeCodewords(int numOfPadeCodewords)
		{
			if(numOfPadeCodewords < 0)
				throw new ArgumentException("Num of pade codewords less than Zero");
			BitList padeCodewords = new BitList();
			for(int numOfP = 1; numOfP <= numOfPadeCodewords; numOfP++)
			{
				if(numOfP % 2 == 1)
					padeCodewords.Add(QRCodeConstantVariable.PadeCodewordsOdd, NumBitsForByte);
				else
					padeCodewords.Add(QRCodeConstantVariable.PadeCodewordsEven, NumBitsForByte);
			}
			return padeCodewords;
		}
		
		private static BitList TerminatorPadding(int numBits)
		{
			BitList terminatorPadding = new BitList();
			terminatorPadding.Add(QRCodeConstantVariable.TerminatorNPaddingBit, numBits);
			return terminatorPadding;
		}
	}
}
