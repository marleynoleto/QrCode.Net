namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal struct Penalty2DecisionNode
    {
        public bool CompareBit { get; private set; }
        public MatrixPoint BitCheckPoint { get; private set; }
        public int IndexJumpValue { get; private set; }

        internal Penalty2DecisionNode(bool compareBit, MatrixPoint bitCheckPoint, int indexJumpValue)
            : this()
        {
            CompareBit = compareBit;
            BitCheckPoint = bitCheckPoint;
            IndexJumpValue = indexJumpValue;
        }
    }
}
