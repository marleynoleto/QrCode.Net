using System.Linq;

namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
    internal static class MatrixScoreCalculator
    {
        internal static BitMatrix GetLowestPenaltyMatrix(this BitMatrix matrix)
        {
            PatternFactory patternFactory = new PatternFactory();
            return 
                patternFactory
                    .AllPatterns()
                    .Select(pattern => matrix.Apply(pattern))
                    .OrderByDescending(PenaltyScore)
                    .First();
        }


        internal static int PenaltyScore(this BitMatrix matrix)
        {
            PenaltyFactory penaltyFactory = new PenaltyFactory();
            return
            	penaltyFactory
            	.AllRules()
            	.Sum(penalty => penalty.PenaltyCalculate(matrix));
        }
    }
}
