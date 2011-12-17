namespace Gma.QrCodeNet.Encoding.EncodingRegion
{
	internal static class BCHCalculator
	{
		internal static int PosMSB(int num)
		{
			return num == 0 ? 0 : BinarySearchPos(num, 0, 32) + 1;
		}
		
		
		private static int BinarySearchPos(int num, int lowBoundary, int highBoundary)
		{
			int mid = (lowBoundary + highBoundary) / 2;
			int shiftResult = num >> mid;
			if(shiftResult == 1)
				return mid;
			else if(shiftResult < 1)
				return BinarySearchPos(num, lowBoundary, mid);
			else
				return BinarySearchPos(num, mid, highBoundary);
		}
		
		/// <summary>
		/// With input number and polynomial number. Method will calculate BCH value and return
		/// </summary>
		/// <param name="num">input number</param>
		/// <param name="poly">Polynomial number</param>
		/// <returns>BCH value</returns>
		internal static int CalculateBCH(int num, int poly)
		{
			int polyMSB = PosMSB(poly);
			//num's length will be old length + new length - 1. 
			//Once divide poly number. BCH number will be one length short than Poly number's length.
			num <<= (polyMSB - 1);
			int numMSB = PosMSB(num);
			while( PosMSB(num) >= polyMSB)
			{
				//left shift Poly number to same level as num. Then xor. 
				//Remove most significant bits of num.
				num ^= poly << (numMSB - polyMSB);
				numMSB = PosMSB(num);
			}
			return num;
		}
		
		
	}
}
