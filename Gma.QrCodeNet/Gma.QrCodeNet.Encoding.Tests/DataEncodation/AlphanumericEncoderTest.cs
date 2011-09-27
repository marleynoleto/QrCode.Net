using System;
using System.Collections.Generic;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.DataEncodation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mode = com.google.zxing.qrcode.decoder.Mode;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    [TestClass]
    public class AlphanumericEncoderTest : EncoderTestBase
    {
        protected override string CsvFileName { get { return "AlphanumericEncoderTestDataSet.csv"; } }

        [TestMethod]
        public override void GenerateTestDataSet()
        {
            base.GenerateTestDataSet();
        }

        protected override string GenerateInputString(int inputSize, Random randomizer)
        {
            //TODO : Must be improved to generate the whole raneg of allowed chars from Chapter 8.4.3 Alphanumeric Mode, Table 5 — Encoding/decoding table for Alphanumeric Mode
            return GenerateInputString(inputSize, randomizer, 'A', 'Z');
        }

        protected override IEnumerable<bool> EncodeUsingReferenceImplementation(string content, int version)
        {
            return EncodeUsingReferenceImplementation(content, version, Mode.ALPHANUMERIC);
        }

        protected override EncoderBase CreateEncoder(int version)
        {
            return new AlphanumericEncoder(version);
        }

        [DeploymentItem("Gma.QrCodeNet.Encoding.Tests\\DataEncodation\\AlphanumericEncoderTestDataSet.csv"), DeploymentItem("Test\\Base.Tests\\AlphanumericEncoderTestDataSet.csv"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\AlphanumericEncoderTestDataSet.csv", "AlphanumericEncoderTestDataSet#csv", DataAccessMethod.Sequential)]
        public override void RunTestsOnDataSet()
        {
            base.RunTestsOnDataSet();
        }
    }
}
