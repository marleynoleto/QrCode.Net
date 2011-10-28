namespace Gma.QrCodeNet.Encoding.Versions
{
	public struct ErrorCorrectionBlock
	{
		public int NumErrorCorrectionBlock { get; private set;}
		
		public int NumDataCodewords { get; private set;}
		
		public ErrorCorrectionBlock(int numErrorCorrectionBlock, int numDataCodewards)
			: this()
		{
			this.NumErrorCorrectionBlock = numErrorCorrectionBlock;
			this.NumDataCodewords = numDataCodewards;
		}
	}
}
