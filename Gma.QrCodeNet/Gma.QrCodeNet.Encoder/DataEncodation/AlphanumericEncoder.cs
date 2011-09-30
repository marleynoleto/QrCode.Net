using System;
using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding.DataEncodation
{
    internal class AlphanumericEncoder : EncoderBase
    {
    	// The original table is defined in the table 5 of ISO/IEC 18004 First Edition 2000-06-15 (P.21)
    	// This array is mapping table 5 towards Unicode 0000 ~ 005F
    	private static readonly int[] ALPHANUMERIC_TABLE = new int[]{- 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, 36, - 1, - 1, - 1, 37, 38, - 1, - 1, - 1, - 1, 39, 40, - 1, 41, 42, 43, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 44, - 1, - 1, - 1, - 1, - 1, - 1, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, - 1, - 1, - 1, - 1, - 1};
		
    	private const int _iMultiply45 = 45;
    	
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
        
        //Combine with TryGetAlphaNumValue. Return false without throw exception
        internal override bool TryGetDataBits(string content, out BitVector dataBits)
        {
        	dataBits = new BitVector();
            
            for (int i = 0; i < content.Length; i += 2)
            {
                int groupLength = Math.Min(2, content.Length-i);
                int value;
                if(!TryGetAlphaNumValue(content, i, groupLength, out value))
                	return false;
                int bitCount = GetBitCountByGroupLength(groupLength);
                dataBits.Append(value, bitCount);
            }
            
            return true;
        }
        
        
        private int GetAlphaNumValue(string content, int startIndex, int length)
        {
        	int Value = 0;
        	int iMultiplyValue = 1;
        	for (int i = 0 ; i < length; i++)
        	{
        		int positionFromEnd = startIndex + length - i - 1;
        		int code = getAlphanumericCode(content[positionFromEnd]);
        		if(code < 0)
        			throw new FormatException();	//Currently throw FormatException
        		Value += code * iMultiplyValue;
        		iMultiplyValue *= _iMultiply45;
        	}
        	return Value;
        }
        
        
        //Try method will return false if any char inside content is not ALphaNum. 
        //Instead of normal method throw exception
        private bool TryGetAlphaNumValue(string content, int startIndex, int length, out int Value)
        {
        	Value = 0;
        	int iMultiplyValue = 1;
        	for (int i = 0 ; i < length; i++)
        	{
        		int positionFromEnd = startIndex + length - i - 1;
        		int code = getAlphanumericCode(content[positionFromEnd]);
        		if(code < 0)
        			return false;
        		Value += code * iMultiplyValue;
        		iMultiplyValue *= _iMultiply45;
        	}
        	return true;
        }
        
        /// <returns> the code point of the table used in alphanumeric mode or
		/// -1 if there is no corresponding code in the table.
		/// </returns>
		internal static int getAlphanumericCode(int code)
		{
			if (code < ALPHANUMERIC_TABLE.Length)
			{
				return ALPHANUMERIC_TABLE[code];
			}
			return - 1;
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
                    return 9;
                case 1:
                    return 11;
                default:
                    return 13;
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
                default:
                    return 11;
            }
        }
    }
}
