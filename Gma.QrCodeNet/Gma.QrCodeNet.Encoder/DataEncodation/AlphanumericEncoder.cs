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
        	int contentLength = content.Length;
            for (int i = 0; i < contentLength; i += 2)
            {
                int groupLength = Math.Min(2, contentLength-i);
                int value = GetAlphaNumValue(content, i, groupLength);
                int bitCount = GetBitCountByGroupLength(groupLength);
                dataBits.Append(value, bitCount);
            }
			return dataBits;
        }
        
    	
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
        
        /// <summary>
        /// BitCount from chapter 8.4.3. P22
        /// </summary>
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