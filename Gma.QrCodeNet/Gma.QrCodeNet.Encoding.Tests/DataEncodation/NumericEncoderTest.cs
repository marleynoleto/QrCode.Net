using System;
using System.Collections.Generic;
using Gma.QrCodeNet.Encoding.DataEncodation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mode = com.google.zxing.qrcode.decoder.Mode;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    [TestClass]
    public class NumericEncoderTest : EncoderTestBase
    {
        protected override string CsvFileName { get { return "NumericEncoderTestDataSet.csv"; }}

        //[TestMethod]
        public override void GenerateTestDataSet()
        {
            base.GenerateTestDataSet();
        }

        protected override string GenerateInputString(int inputSize, Random randomizer)
        {
            return GenerateInputString(inputSize, randomizer, '0', '9');
        }

        protected override IEnumerable<bool> EncodeUsingReferenceImplementation(string content, int version)
        {
            return EncodeUsingReferenceImplementation(content, version, Mode.NUMERIC);
        }

        protected override EncoderBase CreateEncoder(int version)
        {
            return new NumericEncoder(version);
        }

        [DeploymentItem("Gma.QrCodeNet.Encoding.Tests\\DataEncodation\\NumericEncoderTestDataSet.csv"), DeploymentItem("Test\\Base.Tests\\NumericEncoderTestDataSet.csv"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\NumericEncoderTestDataSet.csv", "NumericEncoderTestDataSet#csv", DataAccessMethod.Sequential)]
        public override void RunTestsOnDataSet()
        {
            base.RunTestsOnDataSet();
        }
    }
}
