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
        
        
		
        protected string GenerateRandomKanjiString(int inputSize, Random randomizer)
        {
        	StringBuilder result = new StringBuilder(inputSize);
        	Decoder shiftJisDecoder = System.Text.Encoding.GetEncoding("Shift_JIS").GetDecoder();
        	for(int i = 0; i < inputSize; i++)
        	{
        		
        		int RandomShiftJISChar = RandomGenerateKanjiChar(randomizer);
        		
        		byte[] bytes = ConvertShortToByte(RandomShiftJISChar);
        		int charLength = shiftJisDecoder.GetCharCount(bytes, 0, bytes.Length);
        		
        		if(charLength == 1)
        		{
        			char[] kanjiChar = new char[shiftJisDecoder.GetCharCount(bytes, 0, bytes.Length)];
        			shiftJisDecoder.GetChars(bytes, 0, bytes.Length, kanjiChar, 0);
        			result.Append(kanjiChar[0]);
        		}
        		else
        			throw new ArgumentOutOfRangeException("Random Kanji Char decode fail. Decode result contain more than one char or zero char");
        	}
        	return result.ToString();
        }

        
        private const int FIRST_LOWER_BOUNDARY = 0x889F;
        private const int FIRST_UPPER_BOUNDARY = 0x9FFC;
        
        private const int SECOND_LOWER_BOUNDARY = 0xE040;
		private const int SECOND_UPPER_BOUNDARY = 0xEAA4;
		
        /// <summary>
        /// Random generate a Kanji char
        /// </summary>
        /// <remarks>Kanji Shift JIS char separate to two group</remarks>
        private int RandomGenerateKanjiChar(Random randomizer)
        {
        	return randomizer.Next(0, 2) == 0 ? RandomGenerateKanjiCharFromTable(FIRST_LOWER_BOUNDARY, FIRST_UPPER_BOUNDARY, randomizer)
        		: RandomGenerateKanjiCharFromTable(SECOND_LOWER_BOUNDARY, SECOND_UPPER_BOUNDARY, randomizer);
        }
        
        /// <summary>
        /// Generate Kanji char for specific Group
        /// </summary>
        /// <remarks>
        /// Each group separate to several table. Each table's least significant char will start from 0x40 to 0xFC
        /// </remarks>
        private int RandomGenerateKanjiCharFromTable(int lowerBoundary, int upperBoundary, Random randomizer)
        {
        	int RandomShiftJISChar = 0;
        	do
       		{
        		RandomShiftJISChar = randomizer.Next(lowerBoundary, upperBoundary + 1);
        	} while(isCharOutsideTableRange(RandomShiftJISChar));
        	return RandomShiftJISChar;
        }
        
        /// <summary>
        /// Convert short to byte array.
        /// </summary>
        /// <remarks>
        /// Bitconverter's return value is reversed order. Need to be correct. 
        /// </remarks>
        private byte[] ConvertShortToByte(int value)
        {
        	byte[] converterBytes = BitConverter.GetBytes((short)value);
        	return new byte[]{converterBytes[1], converterBytes[0]};
        }
        
        
        /// <summary>
		/// URL: http://interscript.sourceforge.net/interscript/doc/en_shiftjis_0003.html
		/// Each table start from 40
		/// </summary>
		private const int TABLE_LOWER_BOUNDARY = 0x40;
		/// <summary>
		/// URL: http://interscript.sourceforge.net/interscript/doc/en_shiftjis_0003.html
		/// Each table end with FC
		/// </summary>
		private const int TABLE_UPPER_BOUNDARY = 0xFC;
		
        /// <summary>
        /// Shift JIS is separate to several table. Each table's Least significant byte start from 0x40 to 0xFC
        /// </summary>
        /// <returns>True if outside table range. Else return false</returns>
        private bool isCharOutsideTableRange(int RandomChar)
        {
        	int LeastSignificantByte = RandomChar & 0xFF;
        	return (LeastSignificantByte < TABLE_LOWER_BOUNDARY || LeastSignificantByte > TABLE_UPPER_BOUNDARY);
        }

        protected abstract IEnumerable<bool> EncodeUsingReferenceImplementation(string content, int version);

        protected virtual IEnumerable<bool> EncodeUsingReferenceImplementation(string content, int version, Mode mode)
        {
            // Step 2: Append "bytes" into "dataBits" in appropriate encoding.
            BitVector dataBits = new BitVector();
            EncoderInternal.appendBytes(content, mode, dataBits, "Shift_JIS");

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
