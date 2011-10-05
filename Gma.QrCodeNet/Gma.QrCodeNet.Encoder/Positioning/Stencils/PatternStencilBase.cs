using System;

namespace Gma.QrCodeNet.Encoding.Positioning
{
    internal abstract class PatternStencilBase : BitMatrix
    {
        protected const bool o = false;
        protected const bool x = true;

        public abstract bool[,] Stencil { get; }

        public override bool this[int i, int j]
        {
            get { return Stencil[i, j]; }
            set { throw new NotSupportedException(); }
        }

        public override int Width
        {
            get { return Stencil.GetLength(0); }
        }

        public override int Height
        {
            get { return Stencil.GetLength(1); }
        }

        protected void GetPositions(int version)
        {
            
        }
    }
}