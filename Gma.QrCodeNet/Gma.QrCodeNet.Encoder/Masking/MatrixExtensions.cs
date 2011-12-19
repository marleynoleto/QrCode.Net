using System;
using Gma.QrCodeNet.Encoding.Positioning;

namespace Gma.QrCodeNet.Encoding.Masking
{
    public static class MatrixExtensions
    {
        public static TriStateMatrix Xor(this TriStateMatrix first, BitMatrix second)
        {
        	int width = first.Width;
        	TriStateMatrix maskedMatrix = new TriStateMatrix(width);
        	for(int x = 0; x < width; x++)
        	{
        		for(int y = 0; y < width; y++)
        		{
        			MatrixStatus states = first.MStatus(x, y);
        			switch(states)
        			{
        				case MatrixStatus.NoMask:
        					maskedMatrix[x, y, MatrixStatus.NoMask] = first[x, y];
        					break;
        				case MatrixStatus.Data:
        					maskedMatrix[x, y, MatrixStatus.Data] = first[x, y] ^ second[x, y];
        					break;
        				default:
        					throw new ArgumentException("TristateMatrix has None value cell.", "first");
        			}
        		}
        	}
        	
        	return maskedMatrix;
        }

        public static TriStateMatrix Apply(this TriStateMatrix matrix, Pattern pattern)
        {
            return matrix.Xor(pattern);
        }

        public static TriStateMatrix Apply(this Pattern pattern, TriStateMatrix matrix)
        {
            return matrix.Xor(pattern);
        }
    }
}
