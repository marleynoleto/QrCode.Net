using System;
using System.IO;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.Masking.Scoring;
using NUnit.Framework;
using Gma.QrCodeNet.Encoding.Common;

namespace Gma.QrCodeNet.Encoding.Tests.PenaltyScore
{
	public abstract class PenaltyTestBase
	{
		public virtual void Test_against_reference_implementation(BitMatrix input, PenaltyRules penaltyRule, int expected)
		{
			TestPenaltyRule(input, penaltyRule, expected);
		}
		
		private void TestPenaltyRule(BitMatrix input, PenaltyRules penaltyRule, int expected)
		{
			Penalty penalty = new PenaltyFactory().CreateByRule(penaltyRule);
			int result = penalty.PenaltyCalculate(input);
			
			AssertIntEquals(expected, result);
		}
		
		private static void AssertIntEquals(int expected, int actual)
        {
			if(expected != actual)
				Assert.Fail("Penalty scores are different.\nExpected:{0}Actual:{1}.", expected.ToString(), actual.ToString());
		}
		
	}
}
