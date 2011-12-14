using System;
using System.IO;
using System.Collections.Generic;
using com.google.zxing.qrcode.encoder;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ErrorCorrection
{
	public sealed class ECTestCaseFactory
	{
		private static VersionCodewordsInfo[] s_vcInfo = new VersionCodewordsInfo[]{
			new VersionCodewordsInfo(26, 19, 1),
			new VersionCodewordsInfo(44, 16, 1),
			new VersionCodewordsInfo(100, 36, 4),
			new VersionCodewordsInfo(196, 88, 6),
			new VersionCodewordsInfo(292, 182, 5),
			new VersionCodewordsInfo(1706, 596, 37),
			new VersionCodewordsInfo(3706, 1276, 81)
		};
		
		private const int s_bitLengthForByte = 8;
		
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
		{
			get
			{
				Random randomizer = new Random();
				foreach(VersionCodewordsInfo vc in s_vcInfo)
				{
					for(int numTimes = 0; numTimes < 5; numTimes++)
					{
						BitVector dataCodewords = GenerateDataCodewords(vc.NumDataBytes, randomizer);
						BitVector finalBits = new BitVector();
						EncoderInternal.interleaveWithECBytes(dataCodewords, vc.NumTotalBytes, vc.NumDataBytes, vc.NumECBlocks, finalBits);
						yield return new TestCaseData(dataCodewords, vc, finalBits);
					}
				}
			}
		}
		
		
		private BitVector GenerateDataCodewords(int numDataCodewords, Random randomizer)
		{
			BitVector result = new BitVector();
			for(int numDC = 0; numDC < numDataCodewords; numDC++)
			{
				result.Append((randomizer.Next(0, 256) & 0xFF), s_bitLengthForByte);
			}
			if(result.sizeInBytes() == numDataCodewords)
				return result;
			else
				throw new Exception("Auto generate data codewords fail");
		}
		
		
		private const string s_TxtFileName = "ECTestCase.txt";
		
		public void GenerateTestDataSet()
		{
			string path = Path.Combine(Path.GetTempPath(), s_TxtFileName);
			using(var txtFile = File.CreateText(path))
			{
				foreach(TestCaseData testCase in TestCasesFromReferenceImplementation)
				{
					for(int index = 0; index < 3; index++)
					{
						if(index == 1)
						{
							VersionCodewordsInfo vc = (VersionCodewordsInfo)testCase.Arguments[index];
							txtFile.WriteLine(vc.ToString());
						}
						else
						{
							IEnumerable<bool> ienumBool = (IEnumerable<bool>)testCase.Arguments[index];
							txtFile.WriteLine(ienumBool.To01String());
						}
						
					}
				}
				txtFile.Close();
			}
		}
		
		
		public IEnumerable<TestCaseData> TestCaseFromTxtFile
		{
			get
			{
				string path = Path.Combine(@"ErrorCorrection\TestCases", s_TxtFileName);
				using(var txtFile = File.OpenText(path))
				{
					while (!txtFile.EndOfStream)
					{
						List<IEnumerable<bool>> testCase = new List<IEnumerable<bool>>();
						VersionCodewordsInfo vcInfo = new VersionCodewordsInfo();
						for(int numElement = 0; numElement < 3; numElement++)
						{
							string line = txtFile.ReadLine();
							if(numElement == 1)
								vcInfo = new VersionCodewordsInfo(line);
							else
							{
								testCase.Add(BitVectorTestExtensions.From01String(line));
							}
						}
						yield return new TestCaseData(testCase[0], vcInfo, testCase[1]);
					}
				}
			}
		}
		
		
	}
}
