using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using com.google.zxing.qrcode.decoder;
using com.google.zxing.qrcode.encoder;
using com.google.zxing.common;
using Gma.QrCodeNet.Encoding.DataEncodation;
using Gma.QrCodeNet.Encoding.Versions;
using Gma.QrCodeNet.Encoding.DataEncodation.InputRecognition;
using GMode = Gma.QrCodeNet.Encoding.DataEncodation.Mode;
using Mode = com.google.zxing.qrcode.decoder.Mode;
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
                        IEnumerable<bool> expected = BitVectorTestExtensions.From01String(parts[1]);
                        yield return new TestCaseData(input, expected);
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

                foreach (int inputSize in testInputSizes)
                {
                    string inputString = GenerateRandomInputString(inputSize, randomizer);
                    IEnumerable<bool> result = EncodeUsingReferenceImplementation(inputString);
                    yield return new TestCaseData(inputString, result);
                }
                
            }
        }
        
        public IEnumerable<TestCaseData> TestCasesDataEncodeReferenceImplementation
        {
        	get
            {
                Random randomizer = new Random();
                int[] testInputSizes = new[] { 1, 10, 25, 36, 73, 111, 174, 255 };

                foreach (int inputSize in testInputSizes)
                {
                    string inputString = GenerateRandomInputString(inputSize, randomizer);
                    
                    IEnumerable<bool> result = DataEncodeUsingReferenceImplementation(inputString);
                    yield return new TestCaseData(inputString, result);
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
                    IEnumerable<bool> result = (IEnumerable<bool>)testCaseData.Arguments[1];
                    csvFile.WriteLine(string.Join(s_Semicolon, inputString, result.To01String()));
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
        
        private IEnumerable<bool> DataEncodeUsingReferenceImplementation(string content)
        {
        	//Choose mode
        	RecognitionStruct recognitionResult = InputRecognise.Recognise(content);
        	string encodingName = recognitionResult.EncodingName;
        	Mode mode = ConvertMode(recognitionResult.Mode);
        	
        	//append byte to databits
        	BitVector dataBits = new BitVector();
			EncoderInternal.appendBytes(content, mode, dataBits, encodingName);
			
			int dataBitsLength = dataBits.size();
			VersionControlStruct vcStruct = 
				VersionControl.InitialSetup(dataBitsLength, recognitionResult.Mode, ErrorCorrectionLevel.H, recognitionResult.EncodingName);
			//ECI
			BitVector headerAndDataBits = new BitVector();
			string defaultByteMode = "iso-8859-1";
			if (mode == Mode.BYTE && !defaultByteMode.Equals(encodingName))
			{
				CharacterSetECI eci = CharacterSetECI.getCharacterSetECIByName(encodingName);
				if (eci != null)
				{
					EncoderInternal.appendECI(eci, headerAndDataBits);
				}
			}
			//Mode
			EncoderInternal.appendModeInfo(mode, headerAndDataBits);
			//Char info
			int numLetters = mode.Equals(Mode.BYTE)?dataBits.sizeInBytes():content.Length;
			EncoderInternal.appendLengthInfo(numLetters, vcStruct.Version, mode, headerAndDataBits);
			//Combine with dataBits
			headerAndDataBits.appendBitVector(dataBits);
			
			// Terminate the bits properly.
			EncoderInternal.terminateBits(vcStruct.NumDataBytes, headerAndDataBits);
			
			return headerAndDataBits;
        }
        
        private Mode ConvertMode(GMode mode)
        {
        	switch(mode)
        	{
        		case GMode.Numeric:
        			return Mode.NUMERIC;
        		case GMode.Alphanumeric:
        			return Mode.ALPHANUMERIC;
        		case GMode.EightBitByte:
        			return Mode.BYTE;
        		case GMode.Kanji:
        			return Mode.KANJI;
        		default:
        			throw new ArgumentOutOfRangeException("mode", mode, string.Format("Gma mode doesn't contain {0}", mode));
        	}
        }

        protected abstract IEnumerable<bool> EncodeUsingReferenceImplementation(string content);

        protected virtual IEnumerable<bool> EncodeUsingReferenceImplementation(string content, Mode mode)
        {
            // Step 2: Append "bytes" into "dataBits" in appropriate encoding.
            BitVector dataBits = new BitVector();
            EncoderInternal.appendBytes(content, mode, dataBits, null);


            return dataBits;
        }

    }
}
