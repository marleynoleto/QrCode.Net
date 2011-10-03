using System;
using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding.DataEncodation
{
    internal class NumericEncoder : EncoderBase
    {
        internal NumericEncoder(int version) 
            : base(version)
        {
        }

        internal override Mode Mode
        {
            get { return Mode.Numeric; }
        }

        internal override BitVector GetDataBits(string content)
        {
            BitVector dataBits = new BitVector();
            for (int i = 0; i < content.Length; i += 3)
            {
                int groupLength = Math.Min(3, content.Length-i);
                int value = GetDigitGroupValue(content, i, groupLength);
                int bitCount = GetBitCountByGroupLength(groupLength);
                dataBits.Append(value, bitCount);
            }

            return dataBits;
        }
        
        internal override bool TryGetDataBits(string content, ref BitVector dataBits)
        {
        	//dataBits = new BitVector();
            for (int i = 0; i < content.Length; i += 3)
            {
                int groupLength = Math.Min(3, content.Length-i);
                int value;
                //Use trygetdigitgroupvalue. Return false if content is not numeric.
                if(!TryGetDigitGroupValue(content, i, groupLength, out value))
                	return false;
                int bitCount = GetBitCountByGroupLength(groupLength);
                dataBits.Append(value, bitCount);
            }
            return true;
        }

        protected override int GetBitCountInCharCountIndicator()
        {
            int versionGroup = GetVersionGroup();
            switch (versionGroup)
            {
                case 0:
                    return 10;
                case 1:
                    return 12;
                default:
                    return 14;
            }
        }

        private int GetDigitGroupValue(string content, int startIndex, int length)
        {
            int value=0;
            int iThPowerOf10 = 1;
            for (int i = 0 ; i < length; i++)
            {
                int positionFromEnd = startIndex + length - i - 1;
                int digit = content[positionFromEnd] - '0';
                value += digit * iThPowerOf10;
                iThPowerOf10 *= 10;
            }
            return value;
        }
        
        private bool TryGetDigitGroupValue(string content, int startIndex, int length, out int value)
        {
            value=0;
            int iThPowerOf10 = 1;
            for (int i = 0 ; i < length; i++)
            {
                int positionFromEnd = startIndex + length - i - 1;
                int digit = content[positionFromEnd] - '0';
                //If not numeric. 
                if(digit < 0 || digit > 9)
                	return false;
                value += digit * iThPowerOf10;
                iThPowerOf10 *= 10;
            }
            return true;
        }

        protected int GetBitCountByGroupLength(int groupLength)
        {
            switch (groupLength)
            {
                case 0:
                    return 0;
                case 1:
                    return 4;
                case 2:
                    return 7;
            }
            return 10;
        }
    }
}
