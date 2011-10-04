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

		internal override void GetDataBits(string content, ref BitVector dataBits)
        {
			byte[] contentBytes;
			int contentLength = base.GetDataLength(content);
			
			try {
				//Shift_JIS contain JIS 0201(JIS8) and JIS 0208. 
				//JIS0201 one byte per char. Use for EightBiteByte encode
				//JIS0208 two byte per char. Use for Kanji encode
				contentBytes = System.Text.Encoding.GetEncoding("Shift_JIS").GetBytes(content);
			} catch (Exception e) {
				
				throw e;
			}
			
			int bytesLength = contentBytes.Length;
			
			if(bytesLength / 2 == contentLength && bytesLength % 2 == 0)	//Convert only if all char is 2 bytes length. 
			{
				for(int i = 0; i < bytesLength; i += 2)
				{
					int code = (contentBytes[i] << 8) + (contentBytes[i+1] & 0xff);
					if (code >= 0x8140 && code <= 0x9ffc)	//Formula according to ISO/IEC 18004:2000 Kanji mode Page 24
					{
						code = code - 0x8140;
					}
					else if (code >= 0xe040 && code <= 0xebbf)	//Formula according to ISO/IEC 18004:2000 Kanji mode Page 24
					{
						code = code - 0xc140;
					}
					else
						throw new System.ArgumentException("Invalid byte sequence");
					int encoded = ((code >> 8) * 0xc0) + (code & 0xff);	//Formula according to ISO/IEC 18004:2000 Kanji mode Page 24
					dataBits.appendBits(encoded, 13);	//Formula according to ISO/IEC 18004:2000 Kanji mode Page 25
				}
			}
			else
				throw new System.ArgumentException("Content contain non Kanji char");
			
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
