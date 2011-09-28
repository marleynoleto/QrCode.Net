using System;
using System.Collections.Generic;
using Mode = com.google.zxing.qrcode.decoder.Mode;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    public class AlphanumericEncoderTestCaseFactory : EncoderTestCaseFactoryBase
    {
        protected override string CsvFileName { get { return "AlphanumericEncoderTestDataSet.csv"; } }

        protected override string GenerateRandomInputString(int inputSize, Random randomizer)
        {
            return GenerateRandomInputString(inputSize, randomizer, 'A', 'Z');
        }

        protected override IEnumerable<bool> EncodeUsingReferenceImplementation(string content, int version)
        {
            return EncodeUsingReferenceImplementation(content, version, Mode.ALPHANUMERIC);
        }
    }
}
