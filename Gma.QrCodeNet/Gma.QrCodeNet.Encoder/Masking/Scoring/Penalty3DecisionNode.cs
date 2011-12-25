namespace Gma.QrCodeNet.Encoding.Masking.Scoring
{
	internal struct Penalty3DecisionNode
    {
        public int BitCheckIndex { get; private set; }

        public bool BitValue { get; private set; }

        public int IndexJumpValue { get; private set; }

        internal Penalty3DecisionNode(bool bitValue, int bitCheckIndex, int indexJumpValue) 
            : this()
        {
            BitCheckIndex = bitCheckIndex;
            BitValue = bitValue;
            IndexJumpValue = indexJumpValue;
        }
    }
}
