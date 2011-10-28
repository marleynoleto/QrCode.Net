namespace Gma.QrCodeNet.Encoding.Versions
{
	public struct ErrorCorrectionBlocks
	{
		public int NumErrorCorrectionCodewards { get; private set; }
		
		public int NumBlocks { get; private set; }
		
		public int ErrorCorrectionCodewordsPerBlock { get; private set;}
		
		private ErrorCorrectionBlock[] m_ECBlock;
		
		public ErrorCorrectionBlocks(int numErrorCorrectionCodeWards, ErrorCorrectionBlock ecBlock)
			: this()
		{
			this.NumErrorCorrectionCodewards = numErrorCorrectionCodeWards;
			this.m_ECBlock = new ErrorCorrectionBlock[]{ecBlock};
			
			this.initialize();
		}
		
		public ErrorCorrectionBlocks(int numErrorCorrectionCodeWards, ErrorCorrectionBlock ecBlock1, ErrorCorrectionBlock ecBlock2)
			: this()
		{
			this.NumErrorCorrectionCodewards = numErrorCorrectionCodeWards;
			this.m_ECBlock = new ErrorCorrectionBlock[]{ecBlock1, ecBlock2};
			
			this.initialize();
		}
		
		/// <summary>
		/// Get Error Correction Blocks
		/// </summary>
		internal ErrorCorrectionBlock[] GetECBlocks()
		{
			return m_ECBlock;
		}
		
		/// <summary>
		/// Initialize for NumBlocks and ErrorCorrectionCodewordsPerBlock
		/// </summary>
		private void initialize()
		{
			if(m_ECBlock == null)
				throw new System.ArgumentNullException("ErrorCorrectionBlocks array doesn't contain any value");
			
			NumBlocks = 0;
			int blockLength = m_ECBlock.Length;
			for(int i = 0; i < blockLength; i++)
			{
				NumBlocks += m_ECBlock[i].NumErrorCorrectionBlock;
			}
			
			
			ErrorCorrectionCodewordsPerBlock = NumErrorCorrectionCodewards / NumBlocks;
		}
	}
}
