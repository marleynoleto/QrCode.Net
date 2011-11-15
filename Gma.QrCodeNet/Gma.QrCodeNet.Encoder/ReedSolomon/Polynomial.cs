using System;

namespace Gma.QrCodeNet.Encoding.ReedSolomon
{
	internal sealed class Polynomial
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
		
		
		
		internal int Degree
		{
			get
			{
				return Coefficients.Length - 1;
			}
		}
		
		
		internal Polynomial(int primitive, int[] coefficients)
		{
			int coefficientsLength = coefficients.Length;
			
			if(coefficientsLength == 0 || coefficients == null)
				throw new ArithmeticException("Can not create empty Polynomial");
			
			m_GField = new GaloisField256(primitive);
			
			if(coefficientsLength > 1 && coefficients[0] == 0)
			{
				int firstNonZeroIndex = 1;
				while( firstNonZeroIndex < coefficientsLength && coefficients[firstNonZeroIndex] == 0)
				{
					firstNonZeroIndex++;
				}
				
				if(firstNonZeroIndex == coefficientsLength)
					m_Coefficients = new int[]{0};
				else
				{
					int newLength = coefficientsLength - firstNonZeroIndex;
					m_Coefficients = new int[newLength];
					Array.Copy(coefficients, firstNonZeroIndex, m_Coefficients, 0, newLength);
				}
			}
			else
			{
				Array.Copy(coefficients, m_Coefficients, coefficientsLength);
			}
		}
		
		
		
	}
}
