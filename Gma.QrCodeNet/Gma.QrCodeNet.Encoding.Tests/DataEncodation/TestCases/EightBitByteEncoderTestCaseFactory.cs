using System;
using System.Collections.Generic;
using Mode = com.google.zxing.qrcode.decoder.Mode;
using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    /// <summary>
    /// Description of EightBitByteEncoderTestCaseFactory.
    /// </summary>
    public class EightBitByteEncoderTestCaseFactory : EncoderTestCaseFactoryBase
    {
        protected override string CsvFileName
        {
            get
            {
                return "EightBitByteEncoderTestDataSet.csv";
            }
        }

        protected override string GenerateRandomInputString(int inputSize, Random randomizer)
        {
            return GenerateRandomInputString(inputSize, randomizer, 'ｦ', 'ﾝ');
        }

        protected override IEnumerable<bool> EncodeUsingReferenceImplementation(string content, int version)
        {
            // Step 2: Append "bytes" into "dataBits" in appropriate encoding.
            BitVector dataBits = new BitVector();
            EncoderInternal.appendBytes(content, Mode.BYTE, dataBits, "Shift_JIS");

            // Step 4: Build another bit vector that contains header and data.
            BitVector headerAndDataBits = new BitVector();

            EncoderInternal.appendModeInfo(Mode.BYTE, headerAndDataBits);

            int numLetters = content.Length;
            EncoderInternal.appendLengthInfo(numLetters, version, Mode.BYTE, headerAndDataBits);
            headerAndDataBits.appendBitVector(dataBits);

            return headerAndDataBits;
        }
        
    }
}
