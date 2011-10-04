using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding.DataEncodation
{
    public abstract class EncoderBase
    {
        internal EncoderBase(int version)
        {
            Version = version;
        }

        public int Version { get; private set; }
        internal abstract Mode Mode { get; }
        
        internal virtual BitVector Encode(string content)
        {
//            return
//                GetModeIndicator().Append(
//                    GetCharCountIndicator(GetDataLength(content))).Append(
//                        GetDataBits(content));
			BitVector dataBits = new BitVector();
			this.GetModeIndicator(ref dataBits);
			this.GetCharCountIndicator(GetDataLength(content), ref dataBits);
			GetDataBits(content, ref dataBits);

			return dataBits;
        }

        protected virtual int GetDataLength(string content)
        {
            return content.Length;
        }

        /// <summary>
        /// Returns the bit representation of input data.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal abstract void GetDataBits(string content, ref BitVector dataBits);
        

        /// <summary>
        /// Returns bit representation of <see cref="Mode"/> value.
        /// </summary>
        /// <returns></returns>
        /// <remarks>See Chapter 8.4 Data encodation, Table 2 — Mode indicators</remarks>
        internal void GetModeIndicator(ref BitVector modeIndicatorBits)
        {
            //BitVector modeIndicatorBits = new BitVector();
            modeIndicatorBits.Append((int) this.Mode, 4);
            //return modeIndicatorBits;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterCount"></param>
        /// <returns></returns>
        internal void GetCharCountIndicator(int characterCount, ref BitVector characterCountBits)
        {
            //BitVector characterCountBits = new BitVector();
            int bitCount = GetBitCountInCharCountIndicator();
            characterCountBits.Append(characterCount, bitCount);
            //return characterCountBits;
        }

        /// <summary>
        /// Defines the length of the Character Count Indicator, 
        /// which varies according to themode and the symbol version in use
        /// </summary>
        /// <returns>Number of bits in Character Count Indicator.</returns>
        /// <remarks>
        /// See Chapter 8.4 Data encodation, Table 3 — Number of bits in Character Count Indicator.
        /// </remarks>
        protected abstract int GetBitCountInCharCountIndicator();

        /// <summary>
        /// Used to define length of the Character Count Indicator <see cref="GetBitCountInCharCountIndicator"/>
        /// </summary>
        /// <returns>Returns the 0 based index of the row from Chapter 8.4 Data encodation, Table 3 — Number of bits in Character Count Indicator. </returns>
        protected int GetVersionGroup()
        {
            if (this.Version >= 27)
            {
                return 2;
            }

            if (this.Version >= 10)
            {
                return 1;
            }

            return 0;
        }
    }
}