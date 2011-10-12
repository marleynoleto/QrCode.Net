namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal class Penalty1 : Penalty
	{
		internal override int PenaltyCalculate(BitMatrix matrix)
		{
			return 0;
		}
	}
}
