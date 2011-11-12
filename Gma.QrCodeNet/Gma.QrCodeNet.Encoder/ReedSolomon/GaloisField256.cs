using System;

namespace Gma.QrCodeNet.Encoding.ReedSolomon
{
	/// <summary>
	/// Description of GaloisField256.
	/// </summary>
	internal sealed class GaloisField256
	{
		private int[] antiLogTable;
		private int[] logTable;
		
		private const int s_a = 2;
		
		/// <summary>
		/// ISO/IEC 18004:2006(E) Page 45 Chapter Generating the error correction codewords
		/// Primative Polynomial = Bin 100011101 = Dec 285
		/// </summary>
		private const int s_PrimitivePolynomial_QRCode = 285;
		
		internal GaloisField256(int primitive)
		{
			antiLogTable = new int[256];
			logTable = new int[256];
			
			int gfx = 1;
			
			for(int powers = 0; powers < 256; powers++)
			{
				antiLogTable[powers] = gfx;
				logTable[gfx] = powers;
				gfx *= s_a;
				
				if(gfx > 255)
				{
					gfx ^= primitive;
				}
			}
		}
		
		internal GaloisField256 QRCodeGaloisField
		{
			get
			{
				return new GaloisField256(s_PrimitivePolynomial_QRCode);
			}
		}
		
		/// <returns>
		/// Powers of a in GF table. Where a = 2
		/// </returns>
		internal int Exponent(int PowersOfa)
		{
			return antiLogTable[PowersOfa];
		}
		
		/// <returns>
		/// log ( power of a) in GF table. Where a = 2
		/// </returns>
		internal int Log(int gfValue)
		{
			return logTable[gfValue];
		}
		
		internal int Addition(int gfValueA, int gfValueB)
		{
			return gfValueA ^ gfValueB;
		}
		
		internal int Subtraction(int gfValueA, int gfValueB)
		{
			//Subtraction is same as addition. 
			return this.Addition(gfValueA, gfValueB);
		}
		
		/// <returns>
		/// Product of two values. 
		/// In other words. a multiply b
		/// </returns>
		internal int Product(int gfValueA, int gfValueB)
		{
			return Exponent((Log(gfValueB) + Log(gfValueB)) % 255);
		}
		
		/// <returns>
		/// Quotient of two values. 
		/// In other words. a devided b
		/// </returns>
		internal int Quotient(int gfValueA, int gfValueB)
		{
			return Exponent(Math.Abs(Log(gfValueA) - Log(gfValueB)) % 255);
		}
		
	}
}
