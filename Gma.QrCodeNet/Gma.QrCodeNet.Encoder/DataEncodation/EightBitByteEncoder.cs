using System;
using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding.DataEncodation
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	internal class EightBitByteEncoder : EncoderBase
	{
		private const string _defaultEncoding = "Shift_JIS";
		
		public string Encoding {get; private set;}
		/// <summary>
		/// EightBitByte encoder's encoding will change according to different region
		/// </summary>
		/// <param name="encoding">Default encoding is "Shift_JIS"</param>
		public EightBitByteEncoder(int version, string encoding)
			:base(version)
        {
			Encoding = encoding ?? _defaultEncoding;
        }
		
		
		
		internal override Mode Mode
        {
            get { return Mode.EightBitByte; }
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
		
		private const int EIGHT_BIT_BYTE_BITCOUNT = 8;
		
		internal override BitVector GetDataBits(string content)
        {
			BitVector dataBits = new BitVector();
			
			byte[] contentBytes = EncodeContent(content, Encoding);
			
			int contentLength = base.GetDataLength(content);
			if(contentBytes.Length == contentLength)
			{
				for(int i = 0; i < contentLength; i++)
				{
					dataBits.appendBits(contentBytes[i], EIGHT_BIT_BYTE_BITCOUNT);
				}
			}
			else
				throw new ArgumentOutOfRangeException("EightBiteByte mode will only accept char with one byte length");
			
			return dataBits;
			
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
                case 2:
                    return 16;
                default:
                    throw new InvalidOperationException("Unexpected Version group:" + versionGroup.ToString());
            }
        }
        
        
		
	}
}
