namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal struct Penalty2NodeValue
    {
        public bool CompareBit { get; private set; }
        public Point BitCheckPoint { get; private set; }
        public int IndexJumpValue { get; private set; }

        internal Penalty2NodeValue(bool compareBit, Point bitCheckPoint, int indexJumpValue)
            : this()
        {
            CompareBit = compareBit;
            BitCheckPoint = bitCheckPoint;
            IndexJumpValue = indexJumpValue;
        }
    }
}
