using System;

namespace Gma.QrCodeNet.Encoding.DataEncodation
{
	public struct EncodationStruct
	{
		public Mode Mode{get; private set;}
		
		public string EncodingName{get; private set;}
		
		public EncodationStruct(Mode mode, string encodingName)
			: this()
		{
			this.Mode = mode;
			this.EncodingName = encodingName;
		}
	}
}
