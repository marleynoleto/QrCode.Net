using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.Masking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gma.QrCodeNet.Encoding.Common;

namespace Gma.QrCodeNet.Encoding.Tests.Masking
{
    [TestClass]
    public class MaskPatternTest
    {

        private const string s_Semicolon = ";";
        private const string s_OriginalMatrixColumnName = "OriginalMatrix";
        private const string s_MaskPatternColumnName = "MaskPattern";
        private const string s_ResultingMatrixColumnName = "ResultingMatrix";
        private const string s_CsvFileName = "MaskPatternTestDataSet.csv";

        [TestMethod]
        public void GenerateMaskPatternTestDataSet()
        {
            Random randomizer = new Random();
            int[] matrixSizes = new[] { 1, 10, 113, 256 };

            string path = Path.Combine(Path.GetTempPath(), s_CsvFileName);
            using (var csvFile = File.CreateText(path))
            {
                string columnHeader = string.Join(s_Semicolon, s_OriginalMatrixColumnName, s_MaskPatternColumnName, s_ResultingMatrixColumnName);
                csvFile.WriteLine(columnHeader);

                foreach (int matrixSize in matrixSizes)
                {
                    
                    foreach (int pattern in Enum.GetValues(typeof(MaskPatternType)))
                    {
                        Tuple<BitMatrix, BitMatrix> inputExpectedPair = GenerateRandomMatrix(matrixSize, randomizer, pattern);
                        BitMatrix input = inputExpectedPair.Item1;
                        BitMatrix expected = inputExpectedPair.Item2;
    
                        csvFile.WriteLine(string.Join(s_Semicolon, input.ToBase64(), pattern, expected.ToBase64()));
                    }
                }
                csvFile.Close();
            }
        }

        private Tuple<BitMatrix, BitMatrix> GenerateRandomMatrix(int matrixSize, Random randomizer, int pattern)
        {
            BitVector randomData = GetRandomBits(matrixSize * matrixSize, randomizer);
            ByteMatrix input = new ByteMatrix(matrixSize, matrixSize);
            input.Clear(-1);
            MatrixUtil.TryEmbedDataBits(input, randomData, -1);
            ByteMatrix expected = new ByteMatrix(matrixSize, matrixSize);
            expected.Clear(-1);
            MatrixUtil.TryEmbedDataBits(expected, randomData, pattern);
            return new Tuple<BitMatrix, BitMatrix>(new SimpleBitMatrix(input), new SimpleBitMatrix(expected));
        }

        private BitVector GetRandomBits(int length, Random randomizer)
        {
            BitVector result = new BitVector();
            for(int i=0; i<length / 32; i++)
            {
                int randomBits = randomizer.Next();
                result.appendBits(randomBits, 32);
            }

            int randomRemainingBits = randomizer.Next();
            int remainingBitCount = length%32;
            result.appendBits(randomRemainingBits, remainingBitCount);
            return result;
        }

        public TestContext TestContext
        {
            get;
            set;
        }

        [DeploymentItem("Gma.QrCodeNet.Encoding.Tests\\Masking\\MaskPatternTestDataSet.csv"), DeploymentItem("Test\\Base.Tests\\MaskPatternTestDataSet.csv"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\MaskPatternTestDataSet.csv", "MaskPatternTestDataSet#csv", DataAccessMethod.Sequential)]
        public void TestMethod1()
        {
            string inputMatrixString = TestContext.DataRow[s_OriginalMatrixColumnName].ToString();
            string maskPatternString = TestContext.DataRow[s_MaskPatternColumnName].ToString();
            string expectedMatrixString = TestContext.DataRow[s_ResultingMatrixColumnName].ToString();

            BitMatrix input = BitMatrixTestExtensions.FromBase64(inputMatrixString);
            MaskPatternType patternType = (MaskPatternType)int.Parse(maskPatternString);
            BitMatrix expected = BitMatrixTestExtensions.FromBase64(expectedMatrixString);

            Pattern pattern = new PatternFactory().CreateByType(patternType);

            BitMatrix result = input.Apply(pattern);

            Assert.IsTrue(expected.IsEquivalentTo(result));
        }
    }
}
