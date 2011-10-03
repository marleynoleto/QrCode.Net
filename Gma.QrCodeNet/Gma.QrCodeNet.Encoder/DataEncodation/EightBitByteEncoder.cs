using System;
using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding.DataEncodation
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	internal class EightBitByteEncoder : EncoderBase
	{
		public EightBitByteEncoder(int version) 
            : base(version)
        {
        }
		
		internal override Mode Mode
        {
            get { return Mode.EightBitByte; }
        }

		internal override BitVector GetDataBits(string content)
        {
			BitVector dataBits = new BitVector();
			
			byte[] contentBytes;
			int contentLength = content.Length;
			try {
				contentBytes = System.Text.Encoding.GetEncoding("Shift_JIS").GetBytes(content);
			} catch (Exception e) {
				
				throw e;
			}
			
			if(contentBytes.Length == contentLength)
			{
				for(int i = 0; i < contentLength; i++)
				{
					dataBits.appendBits(contentBytes[i], 8);	//EightBitByte different to Num and AlphaNum. bitCount for each Char is constant 8;
				}
			}
			else
				throw new System.ArgumentException("Content contain non JIS8 char");
			
			return dataBits;
		}
		
		internal override bool TryGetDataBits(string content, ref BitVector dataBits)
        {
			byte[] contentBytes;
			int contentLength = content.Length;
			try {
				contentBytes = System.Text.Encoding.GetEncoding("Shift_JIS").GetBytes(content);
			} catch (Exception) {
				
				return false;
			}
			
			if(contentBytes.Length == contentLength)
			{
				for(int i = 0; i < contentLength; i++)
				{
					dataBits.appendBits(contentBytes[i], 8);
				}
				return true;
			}
			else
				return false;
			
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
                    return 16;
                default:
                    return 16;
            }
        }
        
        
		
	}
}
