using System;
using com.google.zxing.qrcode.encoder;

/*
 * Created by SharpDevelop.
 * User: SilverLancer
 * Date: 30/09/2011
 * Time: 11:54 a.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

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
			for(int i = 0; i < content.Length; i++)
			{
				int value = GetEightBitByteValue(content, i);
				//Reference ISO/IEC 18004:2000(E) B=4+C+8D (P.24) 
				//D=Number of imput data char. C=number of char count indicator(table3) Method:GetBitCountInCharCountIndicator
				dataBits.appendBits(value, 8);	//EightBitByte different to Num and AlphaNum. bitCount for each Char is constant 8;
			}
			return dataBits;
		}
		
		internal override bool TryGetDataBits(string content, ref BitVector dataBits)
        {
			//dataBits = new BitVector();
			for(int i = 0; i < content.Length; i++)
			{
				int value;
				if(!TryGetEightBitByteValue(content, i, out value))
					return false;
				//Reference ISO/IEC 18004:2000(E) B=4+C+8D (P.24) 
				//D=Number of imput data char. C=number of char count indicator(table3) Method:GetBitCountInCharCountIndicator
				dataBits.appendBits(value, 8);	//EightBitByte different to Num and AlphaNum. bitCount for each Char is constant 8;
			}
			return true;
		}
		
		
		private int GetEightBitByteValue(string content, int startIndex)
        {
			int value = content[startIndex];
			
			switch (value) {
				case 0x00A5:
					return 0x5C;	//0x00A5 is JP Yan mark "¥"
				case 0x203E:
					return 0x7E;	//0x203E = "‾" According to Table 6 and JIS X 0201 to Unicode table Url:"http://charset.7jp.net/jis0201.html"
				case 0x5C:
					throw new FormatException();
				case 0x7E:
					throw new FormatException();
				default:
					if(value >= 0x00 && value <= 0x7F)	// 0x00 > 0x7F same as Table 6
						return value;
					else if(value >= 0xFF61 && value <= 0xFF9F)  //0xA1 > 0xDF from Table 6 increased by 0xFEC0 on Unicode 16
						return value - 0xFEC0;
					else
						throw new FormatException();
			}
		}
		
		private bool TryGetEightBitByteValue(string content, int startIndex, out int value)
        {
			value = content[startIndex];
			
			switch (value) {
				case 0x00A5:
					value = 0x5C;	//0x00A5 is JP Yan mark "¥"
					return true;
				case 0x203E:
					value = 0x7E;	//0x203E = "‾" According to Table 6 and JIS X 0201 to Unicode table Url:"http://charset.7jp.net/jis0201.html"
					return true;
				case 0x5C:
					return false;
				case 0x7E:
					return false;
				default:
					if(value >= 0x00 && value <= 0x7F)	// 0x00 > 0x7F same as Table 6
						return true;
					else if(value >= 0xFF61 && value <= 0xFF9F)  //0xA1 > 0xDF from Table 6 increased by 0xFEC0 on Unicode 16
					{
						value -= 0xFEC0;
						return true;
					}
					else
						return false;
			}
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
