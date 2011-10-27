using System;
using System.Collections;
using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding.DataEncodation
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	internal class EightBitByteEncoder : EncoderBase
	{
		private const string _defaultEncoding = "iso-8859-1";
		
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
		
		public EightBitByteEncoder(int version)
			:base(version)
		{
			Encoding = _defaultEncoding;
		}
		
		internal override Mode Mode
        {
            get { return Mode.EightBitByte; }
        }

        protected byte[] EncodeContent(string content, string encoding)
        {
        	byte[] contentBytes;
        	try 
        	{
				contentBytes = System.Text.Encoding.GetEncoding(encoding).GetBytes(content);
			} catch (ArgumentException ex) {
				
				throw ex;
			}
        	return contentBytes;
        }
		
        /// <summary>
        /// Bitcount, Chapter 8.4.4, P.24
        /// </summary>
		private const int EIGHT_BIT_BYTE_BITCOUNT = 8;
		
		internal override BitList GetDataBits(string content)
        {
			BitList dataBits = new BitList();
			
			byte[] contentBytes = EncodeContent(content, Encoding);
			
			int contentLength = base.GetDataLength(content);
			if(contentBytes.Length == contentLength)
			{
				for(int i = 0; i < contentLength; i++)
				{
					dataBits.Add(contentBytes[i], EIGHT_BIT_BYTE_BITCOUNT);
				}
			}
			else
				throw new ArgumentOutOfRangeException("content", "EightBiteByte mode will only accept char with one byte length");
			
			return dataBits;
			
		}
		
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
