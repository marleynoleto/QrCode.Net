using Gma.QrCodeNet.Encoding.Common;
using NUnit.Framework;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.Masking;
using Gma.QrCodeNet.Encoding.Masking.Scoring;


namespace Gma.QrCodeNet.Encoding.Tests.PenaltyScore
{
	public class Penalty2TestCaseFactory : PenaltyScoreTestCaseFactory
	{
		protected override NUnit.Framework.TestCaseData GenerateRandomTestCaseData(int matrixSize, System.Random randomizer, MaskPatternType pattern)
		{
			ByteMatrix matrix;
            
			BitMatrix bitmatrix = GetOriginal(matrixSize, randomizer, out matrix);
			
			ApplyPattern(matrix, (int)pattern);
			
			int expect = MaskUtil.applyMaskPenaltyRule2(matrix);
			
            BitMatrix input = matrix.ToBitMatrix();
            
            return new TestCaseData(input, PenaltyRules.Rule02, expect);
		}
	}
}
