namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	/// <summary>
	/// ISO/IEC 18004:2000 Chapter 8.8.2 Page 52
	/// </summary>
	internal class Penalty4 : Penalty
	{
		/// <summary>
		/// Calculate penalty value for Fourth rule.
		/// Perform O(n) search for available x modules
		/// </summary>
		internal override int PenaltyCalculate(BitMatrix matrix)
		{
			MatrixSize size = matrix.Size;
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
