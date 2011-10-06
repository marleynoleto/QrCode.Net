using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using com.google.zxing.qrcode.decoder;
using com.google.zxing.qrcode.encoder;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    public abstract class EncoderTestCaseFactoryBase
    {

        public IEnumerable<TestCaseData> TestCasesFromCsvFile
        {
            get
            {
                string path = Path.Combine("DataEncodation\\TestCases", CsvFileName);
                using (var reader = File.OpenText(path))
                {
                    string header = reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] parts = line.Split(s_Semicolon[0]);
                        string input = parts[0];
                        int version = int.Parse(parts[1]);
                        IEnumerable<bool> expected = BitVectorTestExtensions.From01String(parts[2]);
                        yield return new TestCaseData(input, version, expected);
                    }
                }
            }
        }

        public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
        {
            get
            {
                Random randomizer = new Random();
                int[] testInputSizes = new[] { 0, 1, 10, 25, 36, 73, 111, 174, 255 };

                for (int version = 1; version < 40; version++)
                {
                    foreach (int inputSize in testInputSizes)
                    {
                        string inputString = GenerateRandomInputString(inputSize, randomizer);
                        IEnumerable<bool> result = EncodeUsingReferenceImplementation(inputString, version);
                        yield return new TestCaseData(inputString, version, result);
                    }
                }
            }
        }

        private const string s_Semicolon = ";";
        private const string s_InputStringColumnName = "InputString";
        private const string s_VersionColumnName = "Version";
        private const string s_ExpectedResultColumnName = "ExpectedResult";
        protected abstract string CsvFileName { get; }

        public virtual void GenerateTestDataSet()
        {
         
            string path = Path.Combine(Path.GetTempPath(), CsvFileName);
            using (var csvFile = File.CreateText(path))
            {
                string columnHeader = string.Join(s_Semicolon, s_InputStringColumnName, s_VersionColumnName, s_ExpectedResultColumnName);
                csvFile.WriteLine(columnHeader);

                foreach (TestCaseData testCaseData in TestCasesFromReferenceImplementation)
                {
                    string inputString = testCaseData.Arguments[0].ToString();
                    int version = int.Parse(testCaseData.Arguments[1].ToString());
                    IEnumerable<bool> result = (IEnumerable<bool>)testCaseData.Arguments[2];
                    csvFile.WriteLine(string.Join(s_Semicolon, inputString, version, result.To01String()));
                }
                csvFile.Close();
            }
        }

        protected abstract string GenerateRandomInputString(int inputSize, Random randomizer);

        protected string GenerateRandomInputString(int inputSize, Random randomizer, char startCharCode, char endCharCode)
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

    }
}
