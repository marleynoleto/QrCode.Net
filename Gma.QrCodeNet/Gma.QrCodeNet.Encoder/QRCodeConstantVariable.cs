namespace Gma.QrCodeNet.Encoding
{
    public static class QRCodeConstantVariable
    {
        public const int MinVersion = 1;
        public const int MaxVersion = 40;
        
        public const string DefaultEncoding = "iso-8859-1"; 
        public const string UTF8Encoding = "utf-8";
        
        /// <summary>
        /// URL:http://en.wikipedia.org/wiki/Byte-order_mark
        /// </summary>
        public static byte[] UTF8ByteOrderMark
        {
        	get
        	{
        		return new byte[]{0xEF, 0xBB, 0xBF};
        	}
        }
    }
}
