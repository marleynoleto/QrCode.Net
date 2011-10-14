using Gma.QrCodeNet.Encoding.Masking.Scoring;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.PenaltyScore
{
	[TestFixture]
	public class PenaltyTotalTest : PenaltyTestBase
	{
		[Test, TestCaseSource(typeof(PenaltyTotalTestCaseFactory), "TestCasesFromReferenceImplementation")]
		public override void Test_against_reference_implementation(BitMatrix input, PenaltyRules penaltyRule, int expected)
		{
			int result = MatrixScoreCalculator.PenaltyScore(input);
			AssertIntEquals(expected, result, input);
			
			
		}
	}
}
