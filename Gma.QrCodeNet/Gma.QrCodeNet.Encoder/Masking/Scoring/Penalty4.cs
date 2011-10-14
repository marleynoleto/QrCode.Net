namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal class Penalty4 : Penalty
	{
		internal override int PenaltyCalculate(BitMatrix matrix)
		{
			Size size = matrix.Size;
			int DarkBitCount = 0;
			
			for(int j = 0; j < size.Height; j++)
			{
				for(int i = 0; i < size.Width; i++)
				{
					if(matrix[i, j])
						DarkBitCount++;
				}
			}
			
			int MatrixCount = size.Width * size.Height;
			
			double ratio = (double)DarkBitCount / MatrixCount;
			
			return System.Math.Abs((int)(ratio*100 -50)) / 5 * 10;
			
		}
	}
}
