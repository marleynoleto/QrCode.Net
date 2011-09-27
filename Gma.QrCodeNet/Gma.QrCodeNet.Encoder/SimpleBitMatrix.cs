using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gma.QrCodeNet.Encoding
{
    internal class SimpleBitMatrix : BitMatrix
    {
        private readonly bool[,] m_InternalMatrix;

        public SimpleBitMatrix(int width)
        {
            m_InternalMatrix = new bool[width,width];
        }

        public override bool this[int i, int j]
        {
            get { return m_InternalMatrix[i, j]; }
        }

        internal void Set(int i, int j, bool value)
        {
            m_InternalMatrix[i, j] = value;
        }

        public override int Width
        {
            get { return m_InternalMatrix.GetLength(0); }
        }
    }
}
