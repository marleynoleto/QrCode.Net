using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gma.QrCodeNet.Encoding.Tests
{
    [TestClass]
    public class EndToEndSmokeTests
    {

        private const string s_Semicolon = ";";
        private const string s_InputStringColumnName = "InputString";
        private const string s_ErrorCorrectionLevelColumnName = "ErrorCorrectionLevel";
        private const string s_MatrixBitsColumnName = "MatrixBits";

        //[TestMethod]
        public void GenerateDataSet1()
        {
            Random randomizer = new Random();
            int[] testInputSizes = new[] { 0, 1, 10, 25, 36, 73, 111, 174, 255 };
            
            string path = Path.Combine(Path.GetTempPath(), "DataSet1.csv");
            using (var csvFile = File.CreateText(path))
            {
                string columnHeader = string.Join(s_Semicolon, s_InputStringColumnName, s_ErrorCorrectionLevelColumnName, s_MatrixBitsColumnName);
                csvFile.WriteLine(columnHeader);

                foreach (int inputSize in testInputSizes)
                {
                    string inputString = GetInputString(inputSize, randomizer);
                    foreach (ErrorCorrectionLevel level in Enum.GetValues(typeof(ErrorCorrectionLevel)))
                    {
                        QrEncoder encoder = new QrEncoder(level);
                        BitMatrix matrix = encoder.Encode(inputString).Matrix;
                        csvFile.WriteLine(string.Join(s_Semicolon, inputString, level, matrix.ToBase64()));
                    }
                }
                csvFile.Close();
            }
        }

        private static string GetInputString(int inputSize, Random randomizer)
        {
            const char startCharCode = '(';
            const char endCharCode = '~';
            StringBuilder result = new StringBuilder(inputSize);
            for (int i = 0; i < inputSize; i++)
            {
                char randomChar;
                do
                {
                    randomChar = (char) randomizer.Next(startCharCode, endCharCode);
                } while (randomChar == s_Semicolon[0]);

                result.Append(randomChar);
            }
            return result.ToString();
        }

        public TestContext TestContext
        {
            get;
            set;
        }

        [DeploymentItem("Gma.QrCodeNet.Encoding.Tests\\DataSet1.csv"), DeploymentItem("Test\\Base.Tests\\TestData.csv"), TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\DataSet1.csv", "DataSet1#csv", DataAccessMethod.Sequential)]
        public void TestDataSet1()
        {
            string inputString = TestContext.DataRow[s_InputStringColumnName].ToString();
            string errorCorrectionLevelString = TestContext.DataRow[s_ErrorCorrectionLevelColumnName].ToString();
            string matrixBitsString = TestContext.DataRow[s_MatrixBitsColumnName].ToString();

            ErrorCorrectionLevel level = (ErrorCorrectionLevel)Enum.Parse(typeof (ErrorCorrectionLevel), errorCorrectionLevelString);
            BitMatrix expectedMatrix = BitMatrixTestExtensions.FromBase64(matrixBitsString);

            QrEncoder encoder = new QrEncoder(level);
            BitMatrix resultMatrix = encoder.Encode(inputString).Matrix;

            Assert.IsTrue(expectedMatrix.IsEquivalentTo(resultMatrix));
        }
    }
}
