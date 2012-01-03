using System;

namespace Gma.QrCodeNet.Encoding
{
    public class TriStateMatrix : BitMatrix
    {
        private readonly StateMatrix m_stateMatrix;
        
        private readonly bool[,] m_InternalArray;

        private readonly int m_Width;

        public TriStateMatrix(int width)
        {
            m_stateMatrix = new StateMatrix(width);
            m_InternalArray = new bool[width, width];
            m_Width = width;
        }

        public override bool this[int i, int j]
        {
            get
            {
                return m_InternalArray[i, j];
            }
            set
            {
            	if (MStatus(i, j) == MatrixStatus.None || MStatus(i, j) == MatrixStatus.NoMask)
            	{
            		throw new InvalidOperationException(string.Format("The value of cell [{0},{1}] is not set or is Stencil.", i, j));
            	}
                m_InternalArray[i, j] = value;
            }
        }
        
        public bool this[int i, int j, MatrixStatus mstatus]
        {
        	set
        	{
        		m_stateMatrix[i, j] = mstatus;
        		m_InternalArray[i, j] = value;
        	}
        }

        internal MatrixStatus MStatus(int i, int j)
        {
            return m_stateMatrix[i, j];
        }

        internal MatrixStatus MStatus(MatrixPoint point)
        {
            return MStatus(point.X, point.Y);
        }
        
         public override int Height
        {
            get { return Width; }
        }

        public override int Width
        {
            get { return m_Width; }
        }
    }
}
