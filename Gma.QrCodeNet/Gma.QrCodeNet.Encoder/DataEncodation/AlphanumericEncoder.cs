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
            throw new NotImplementedException();
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
    }
}
