using System;

namespace Gma.QrCodeNet.Encoding.ReedSolomon
{
	internal class Polynomial
	{
		
		private int[] m_Coefficients;
		
		internal int[] Coefficients
		{
			get
			{
				return m_Coefficients;
			}
		}
		
		private GaloisField256 m_GField;
		
		internal GaloisField256 GField
		{
			get
			{
				return m_GField;
			}
		}
		
		
		
		
		
	}
}
