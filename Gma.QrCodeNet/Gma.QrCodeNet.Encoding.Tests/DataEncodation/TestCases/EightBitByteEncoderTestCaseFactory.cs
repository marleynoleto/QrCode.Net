using System;
using System.Collections.Generic;
using Mode = com.google.zxing.qrcode.decoder.Mode;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
	/// <summary>
	/// Description of EightBitByteEncoderTestCaseFactory.
	/// </summary>
	public class EightBitByteEncoderTestCaseFactory : EncoderTestCaseFactoryBase
	{
		protected override string CsvFileName {
			get {
				throw new NotImplementedException();
			}
		}
		
		protected override string GenerateRandomInputString(int inputSize, Random randomizer)
        {
			return GenerateRandomInputString(inputSize, randomizer, 'ｦ', 'ﾝ');
        }
		
		protected override IEnumerable<bool> EncodeUsingReferenceImplementation(string content, int version)
        {
            return EncodeUsingReferenceImplementation(content, version, Mode.BYTE);
        }
	}
}
