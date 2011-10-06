using System;
using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding.DataEncodation
{
	/// <summary>
	/// Description of KanjiEncoder.
	/// </summary>
	internal class KanjiEncoder : EncoderBase
	{
		public KanjiEncoder(int version)
			:base(version)
		{
		}
		
		internal override Mode Mode
        {
            get { return Mode.Kanji; }
        }

		private const int KANJI_BITCOUNT = 13;
		
		internal override BitVector GetDataBits(string content)
        {
			BitVector dataBits = new BitVector();
			
			byte[] contentBytes = EncodeContent(content, "Shift_JIS");
			int contentLength = base.GetDataLength(content);
			
			
			int bytesLength = contentBytes.Length;
			
			if(bytesLength == contentLength*2)
			{
				for(int i = 0; i < bytesLength; i += 2)
				{
					int encoded = ConvertShiftJIS(contentBytes[i], contentBytes[i+1]);
					dataBits.appendBits(encoded, KANJI_BITCOUNT);	//Formula according to ISO/IEC 18004:2000 Kanji mode Page 25
				}
			}
			else
				throw new System.ArgumentOutOfRangeException("Each char must be two byte length");
			
			return dataBits;
			
		}
		
		/// <summary>
        /// Encode content to specific encoding byte array
        /// </summary>
        /// <param name="encoding">
        /// The code page name of the preferred encoding.
        /// Possible values are listed in the Name column of the table that appears in the Encoding class topic
        /// </param>
        /// <returns>Byte array</returns>
        protected byte[] EncodeContent(string content, string encoding)
        {
        	byte[] contentBytes;
        	try 
        	{
				contentBytes = System.Text.Encoding.GetEncoding(encoding).GetBytes(content);
			} catch (ArgumentException e) {
				
				throw e;
			}
        	return contentBytes;
        }
		
		private const int FST_GROUP_LOWER_BOUNDARY = 0x8140;
		private const int FST_GROUP_UPPER_BOUNDARY = 0x9FFC;
		private const int FST_GROUP_SUBTRACT_VALUE = 0x8140;
		
		private const int SEC_GROUP_LOWER_BOUNDARY = 0xE040;
		private const int SEC_GROUP_UPPER_BOUNDARY = 0xEBBF;
		private const int SEC_GROUP_SUBTRACT_VALUE = 0xC140;
		
		
		/// <summary>
		/// Multiply value for Most significant byte.
		/// </summary>
		private const int MULTIPLY_FOR_msb = 0xC0;
		
		/// <summary>
		/// Shift JIS two byte encoding Compacte to 13 bit binary codewords 
		/// </summary>
		/// <returns>13 bit binary codewards</returns>
		/// <remarks>
		/// See Chapter 8.4.5 P.24 Kanji Mode
		/// </remarks>
		private int ConvertShiftJIS(byte FirstByte, byte SecondByte)
		{
			int ShiftJISValue = (FirstByte << 8) + (SecondByte & 0xff);
			int Subtracted = -1;
			if (ShiftJISValue >= FST_GROUP_LOWER_BOUNDARY && ShiftJISValue <= FST_GROUP_UPPER_BOUNDARY)
			{
				Subtracted = ShiftJISValue - FST_GROUP_SUBTRACT_VALUE;
			}
			else if (ShiftJISValue >= SEC_GROUP_LOWER_BOUNDARY && ShiftJISValue <= SEC_GROUP_UPPER_BOUNDARY)
			{
				Subtracted = ShiftJISValue - SEC_GROUP_SUBTRACT_VALUE;
			}
			else 
				throw new System.ArgumentOutOfRangeException("Char is not inside acceptable range.");
				
			return ((Subtracted >> 8) * MULTIPLY_FOR_msb) + (Subtracted & 0xFF);
		}
		
		/// <summary>
        /// Defines the length of the Character Count Indicator, 
        /// which varies according to themode and the symbol version in use
        /// </summary>
        /// <returns>Number of bits in Character Count Indicator.</returns>
        /// <remarks>
        /// See Chapter 8.4 Data encodation, Table 3 — Number of bits in Character Count Indicator.
        /// </remarks>
        protected override int GetBitCountInCharCountIndicator()
        {
        	int versionGroup = GetVersionGroup();
            switch (versionGroup)
            {
                case 0:
                    return 8;
                case 1:
                    return 10;
                default:
                    return 12;
            }
        }
		
	}
}
