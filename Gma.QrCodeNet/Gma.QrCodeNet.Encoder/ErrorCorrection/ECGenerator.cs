using System;
using System.Linq;
using System.Collections.Generic;
using Gma.QrCodeNet.Encoding.common;
using Gma.QrCodeNet.Encoding.Versions;
using Gma.QrCodeNet.Encoding.ReedSolomon;

namespace Gma.QrCodeNet.Encoding.ErrorCorrection
{
	internal static class ECGenerator
	{
		/// <summary>
		/// Generate error correction blocks. Then out put with codewords BitList
		/// ISO/IEC 18004/2006 P45, 46. Chapter 6.6 Constructing final message codewords sequence.
		/// </summary>
		/// <param name="dataCodewords">Datacodewords from DataEncodation.DataEncode</param>
		/// <param name="numTotalBytes">Total number of bytes</param>
		/// <param name="numDataBytes">Number of data bytes</param>
		/// <param name="numECBlocks">Number of Error Correction blocks</param>
		/// <returns>codewords BitList contain datacodewords and ECCodewords</returns>
		internal static BitList FillECCodewords(BitList dataCodewords, int numTotalBytes, int numDataBytes, int numECBlocks)
		{
			byte[] dataCodewordsByte = BitListExtensions.ToByteArray(dataCodewords);
			
			int ecBlockGroup2 = numTotalBytes % 5;
			int ecBlockGroup1 = numECBlocks - ecBlockGroup2;
			int numDataBytesGroup1 = numDataBytes / numECBlocks;
			int numDataBytesGroup2 = numDataBytesGroup1 + 1;
			
			int ecBytesPerBlock = (numTotalBytes - numDataBytes) / numECBlocks;
			
			int dataBytesOffset = 0;
			byte[][] dByteJArray = new byte[numECBlocks][];
			byte[][] ecByteJArray = new byte[numECBlocks][];
			
			GaloisField256 gf256 = GaloisField256.QRCodeGaloisField;
			GeneratorPolynomial generator = new GeneratorPolynomial(gf256);
			
			for(int blockID = 0; blockID < numECBlocks; blockID++)
			{
				if(blockID < ecBlockGroup1)
				{
					Array.Copy(dataCodewordsByte, dataBytesOffset, dByteJArray[blockID], 0, numDataBytesGroup1);
					dataBytesOffset += numDataBytesGroup1;
				}
				else
				{
					Array.Copy(dataCodewordsByte, dataBytesOffset, dByteJArray[blockID], 0, numDataBytesGroup2);
					dataBytesOffset += numDataBytesGroup2;
				}
				
				ecByteJArray[blockID] = ReedSolomonEncoder.Encode(dByteJArray[blockID], ecBytesPerBlock, generator);
			}
			if(numDataBytes != dataBytesOffset)
				throw new ArgumentException("Data bytes does not match offset");
			
			BitList codewords = new BitList();
			
			int maxDataLength = ecBlockGroup1 == numECBlocks ? numDataBytesGroup1 : numDataBytesGroup2;
			
			for(int dataID = 0; dataID < maxDataLength; dataID++)
			{
				for(int blockID = 0; blockID < numECBlocks; blockID++)
				{
					if( !(dataID == numDataBytesGroup1 && blockID < ecBlockGroup1) )
						codewords.Add((int)dByteJArray[blockID][dataID], 8);
				}
			}
			
			for(int ECID = 0; ECID < ecBytesPerBlock; ECID++)
			{
				for(int blockID = 0; blockID < numECBlocks; blockID++)
				{
					codewords.Add((int)ecByteJArray[blockID][ECID], 8);
				}
			}
			
			if( numTotalBytes != codewords.Count >> 3)
				throw new ArgumentException(string.Format("total bytes: {0}, actual bits: {1}", numTotalBytes, codewords.Count));
			
			return codewords;
			
		}
		
		
		
		
	}
}
