using System;

namespace Gma.QrCodeNet.Encoding.Positioning
{
    public class TriStateMatrix : SimpleBitMatrix
    {
        private readonly StateMatrix m_stateMatrix;

        public TriStateMatrix(int width)
            : base(width)
        {
            m_stateMatrix = new StateMatrix(width);
        }

        public override bool this[int i, int j]
        {
            get
            {
            	if (MStatus(i, j) == MatrixStatus.None)
                {
                    throw new InvalidOperationException(string.Format("The value of cell [{0},{1}] is not set.", i, j));
                }
                return base[i, j];
            }
            set
            {
            	if (MStatus(i, j) == MatrixStatus.None || MStatus(i, j) == MatrixStatus.NoMask)
            	{
            		throw new InvalidOperationException(string.Format("The value of cell [{0},{1}] is not set or is Stencil.", i, j));
            	}
                base[i, j] = value;
            }
        }
        
        public bool this[int i, int j, MatrixStatus mstatus]
        {
        	set
        	{
        		m_stateMatrix[i, j] = mstatus;
        		base[i, j] = value;
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
    }
}
