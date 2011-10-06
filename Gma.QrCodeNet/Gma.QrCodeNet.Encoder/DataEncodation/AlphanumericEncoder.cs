using System;
using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding.DataEncodation
{
    internal class AlphanumericEncoder : EncoderBase
    {
    	
    	
        public AlphanumericEncoder(int version) 
            : base(version)
        {
        }

        internal override Mode Mode
        {
            get { return Mode.Alphanumeric; }
        }

        internal override BitVector GetDataBits(string content)
        {
        	BitVector dataBits = new BitVector();
            for (int i = 0; i < content.Length; i += 2)
            {
                int groupLength = Math.Min(2, content.Length-i);
                int value = GetAlphaNumValue(content, i, groupLength);
                int bitCount = GetBitCountByGroupLength(groupLength);
                dataBits.Append(value, bitCount);
            }
			return dataBits;
        }
        
    	/// <summary>
    	/// The original table is defined in the table 5 of ISO/IEC 18004 First Edition 2000-06-15 (P.21)
    	/// This array is mapping table 5 towards Unicode 0000 ~ 005F
    	/// </summary>
    	//private static readonly int[] ALPHANUMERIC_TABLE = new int[]{- 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, 36, - 1, - 1, - 1, 37, 38, - 1, - 1, - 1, - 1, 39, 40, - 1, 41, 42, 43, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 44, - 1, - 1, - 1, - 1, - 1, - 1, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, - 1, - 1, - 1, - 1, - 1};
		
    	/// <summary>
    	/// Constant from Chapter 8.4.3 Alphanumeric Mode. P.21
    	/// </summary>
    	private const int MULTIPLY_FIRST_CHAR = 45;
    	
        private int GetAlphaNumValue(string content, int startIndex, int length)
        {
        	int Value = 0;
        	int iMultiplyValue = 1;
        	for (int i = 0 ; i < length; i++)
        	{
        		int positionFromEnd = startIndex + length - i - 1;
        		int code = AlphanumericTable.ConvertAlphaNumChar(content[positionFromEnd]);
        		if(code < 0)
        			throw new ArgumentOutOfRangeException("Char inside content is not AlphaNumeric");
        		Value += code * iMultiplyValue;
        		iMultiplyValue *= MULTIPLY_FIRST_CHAR;
        	}
        	return Value;
        }
        
        
        /// <returns> 
        /// the code point of the table used in alphanumeric mode or
		/// -1 if there is no corresponding code in the table.
		/// </returns>
//		internal static int getAlphanumericCode(int code)
//		{
//			return code < ALPHANUMERIC_TABLE.Length ? ALPHANUMERIC_TABLE[code] : -1;
//		}
        

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
                    return 9;
                case 1:
                    return 11;
                case 2:
                    return 13;
                default:
                    throw new InvalidOperationException("Unexpected Version group:" + versionGroup.ToString());
            }
        }
        
        protected int GetBitCountByGroupLength(int groupLength)
        {
            switch (groupLength)
            {
                case 0:
                    return 0;
                case 1:
                    return 6;
                case 2:
                    return 11;
                default:
                    throw new InvalidOperationException("Unexpected group length:" + groupLength.ToString());
            }
        }
    }
}