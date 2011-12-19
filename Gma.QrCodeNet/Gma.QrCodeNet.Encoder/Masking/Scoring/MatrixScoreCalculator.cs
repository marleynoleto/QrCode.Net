using System.Linq;
using Gma.QrCodeNet.Encoding.Positioning;

namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
    internal static class MatrixScoreCalculator
    {
        internal static BitMatrix GetLowestPenaltyMatrix(this TriStateMatrix matrix)
        {
            PatternFactory patternFactory = new PatternFactory();
            return 
                patternFactory
                    .AllPatterns()
                    .Select(pattern => matrix.Apply(pattern))
            		.OrderByDescending(patternedMatrix => patternedMatrix.PenaltyScore())
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
