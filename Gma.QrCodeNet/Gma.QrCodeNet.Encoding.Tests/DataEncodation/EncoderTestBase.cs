using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.DataEncodation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mode = com.google.zxing.qrcode.decoder.Mode;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    public abstract class EncoderTestBase
    {
        private const string s_Semicolon = ";";
        private const string s_InputStringColumnName = "InputString";
        private const string s_VersionColumnName = "Version";
        private const string s_ExpectedResultColumnName = "ExpectedResult";
        protected abstract string CsvFileName { get; }

        public virtual void GenerateTestDataSet()
        {
            Random randomizer = new Random();
            int[] testInputSizes = new[] { 0, 1, 10, 25, 36, 73, 111, 174, 255 };

            string path = Path.Combine(Path.GetTempPath(), CsvFileName);
            using (var csvFile = File.CreateText(path))
            {
                string columnHeader = string.Join(s_Semicolon, s_InputStringColumnName, s_VersionColumnName, s_ExpectedResultColumnName);
                csvFile.WriteLine(columnHeader);

                for (int version = 1; version < 40; version++)
                {
                    foreach (int inputSize in testInputSizes)
                    {
                        string inputString = GenerateInputString(inputSize, randomizer);
                        IEnumerable<bool> result = EncodeUsingReferenceImplementation(inputString, version, Mode.ALPHANUMERIC);
                        csvFile.WriteLine(string.Join(s_Semicolon, inputString, version, result.To01String()));
                    }
                }
                csvFile.Close();
            }
        }

        protected abstract string GenerateInputString(int inputSize, Random randomizer);

        protected string GenerateInputString(int inputSize, Random randomizer, char startCharCode, char endCharCode)
        {
            StringBuilder result = new StringBuilder(inputSize);
            for (int i = 0; i < inputSize; i++)
            {
                char randomChar;
                do
                {
                    randomChar = (char)randomizer.Next(startCharCode, endCharCode);
                } while (randomChar == s_Semicolon[0]);

                result.Append(randomChar);
            }
            return result.ToString();
        }

        protected abstract IEnumerable<bool> EncodeUsingReferenceImplementation(string content, int version);

        protected virtual IEnumerable<bool> EncodeUsingReferenceImplementation(string content, int version, Mode mode)
        {
            // Step 2: Append "bytes" into "dataBits" in appropriate encoding.
            BitVector dataBits = new BitVector();
            EncoderInternal.appendBytes(content, mode, dataBits, null);

            // Step 4: Build another bit vector that contains header and data.
            BitVector headerAndDataBits = new BitVector();

            EncoderInternal.appendModeInfo(mode, headerAndDataBits);

            int numLetters = content.Length;
            EncoderInternal.appendLengthInfo(numLetters, version, mode, headerAndDataBits);
            headerAndDataBits.appendBitVector(dataBits);

            return headerAndDataBits;
        }

        public TestContext TestContext
        {
            get;
            set;
        }

        public virtual void RunTestsOnDataSet()
        {
            string inputString = TestContext.DataRow[s_InputStringColumnName].ToString();
            string versionString = TestContext.DataRow[s_VersionColumnName].ToString();
            string expectedResult01String = TestContext.DataRow[s_ExpectedResultColumnName].ToString();

            int version = int.Parse(versionString);
            IEnumerable<bool> expectedResult = BitVectorTestExtensions.From01String(expectedResult01String);

            EncoderBase target = CreateEncoder(version);
            IEnumerable<bool> actualResult = target.Encode(inputString);

            CollectionAssert.AreEquivalent(expectedResult.ToList(), actualResult.ToList());
        }

        protected abstract EncoderBase CreateEncoder(int version);
    }
}