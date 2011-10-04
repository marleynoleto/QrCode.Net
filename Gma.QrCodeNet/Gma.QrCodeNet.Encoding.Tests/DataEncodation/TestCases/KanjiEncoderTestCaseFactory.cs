using System;
using System.Collections.Generic;
using Mode = com.google.zxing.qrcode.decoder.Mode;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
	/// <summary>
	/// Description of KanjiEncoderTestCaseFactory.
	/// </summary>
	public class KanjiEncoderTestCaseFactory : EncoderTestCaseFactoryBase
	{
		protected override string CsvFileName { get { return "KanjiEncoderTestDataSet.csv"; } }
		
		protected override string GenerateRandomInputString(int inputSize, Random randomizer)
        {
			return GenerateRandomKanjiString(inputSize, randomizer);
        }
		
		protected override IEnumerable<bool> EncodeUsingReferenceImplementation(string content, int version)
        {
            return EncodeUsingReferenceImplementation(content, version, Mode.KANJI);
        }
		
	}
}
