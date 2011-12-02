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
		private const string _defaultEncoding = QRCodeConstantVariable.DefaultEncoding;
		
		public string Encoding {get; private set;}
		/// <summary>
		/// EightBitByte encoder's encoding will change according to different region
		/// </summary>
		/// <param name="encoding">Default encoding is "iso-8859-1"</param>
		public EightBitByteEncoder(string encoding)
			:base()
        {
			Encoding = encoding ?? _defaultEncoding;
        }
		
		public EightBitByteEncoder()
			:base()
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
			if(!ECISet.ContainsECIName(Encoding.ToLower()))
			{
				throw new ArgumentOutOfRangeException("Encoding", 
				                                      Encoding, 
				                                      "Current ECI table does not support this encoding. Please check ECISet class for more info");
			}
			
			byte[] contentBytes = EncodeContent(content, Encoding);
			
			int contentLength = base.GetDataLength(content);
			
			return GetDataBitsByByteArray(contentBytes, Encoding);
		}
		
		internal BitList GetDataBitsByByteArray(byte[] encodeContent, string encodingName)
		{
			BitList dataBits = new BitList();
			//Current plan for UTF8 support is put Byte order Mark in front of content byte. 
			//Also include ECI header before encoding header. Which will be add with encoding header.
			if(encodingName == "utf-8")
			{
				byte[] utf8BOM = QRCodeConstantVariable.UTF8ByteOrderMark;
				int utf8BOMLength = utf8BOM.Length;
				for(int index = 0; index < utf8BOMLength; index++)
				{
					dataBits.Add(utf8BOM[index], EIGHT_BIT_BYTE_BITCOUNT);
				}
				
			}
			
			int encodeContentLength = encodeContent.Length;
			
			for(int index = 0; index < encodeContentLength; index++)
			{
				dataBits.Add(encodeContent[index], EIGHT_BIT_BYTE_BITCOUNT);
			}
			return dataBits;
		}
		
        protected override int GetBitCountInCharCountIndicator(int version)
        {
            return CharCountIndicatorTable.GetBitCountInCharCountIndicator(Mode.EightBitByte, version);
        }
        
        
		
	}
}
