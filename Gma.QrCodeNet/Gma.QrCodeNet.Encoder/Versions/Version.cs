namespace Gma.QrCodeNet.Encoding.Versions
{
	public struct Version
	{
		public int VersionNum { get; private set;}
		
		public int TotalCodewords { get; private set;}
		
		public int DimensionForVersion { get; private set;}
		
		private ErrorCorrectionBlocks[] m_ECBlocks;
		
		/// <param name="ecblocks1">L</param>
		/// <param name="ecblocks2">M</param>
		/// <param name="ecblocks3">Q</param>
		/// <param name="ecblocks4">H</param>
		public Version(int versionNum, int totalCodewords, ErrorCorrectionBlocks ecblocksL, ErrorCorrectionBlocks ecblocksM, ErrorCorrectionBlocks ecblocksQ, ErrorCorrectionBlocks ecblocksH)
			: this()
		{
			this.VersionNum = versionNum;
			this.TotalCodewords = totalCodewords;
			this.m_ECBlocks = new ErrorCorrectionBlocks[]{ecblocksL, ecblocksM, ecblocksQ, ecblocksH};
			this.DimensionForVersion = 17 + versionNum * 4;
		}
		
		/// <summary>
		/// Get Error Correction Blocks by level
		/// </summary>
		//[method
		public ErrorCorrectionBlocks GetECBlocksByLevel(Gma.QrCodeNet.Encoding.ErrorCorrectionLevel ECLevel)
		{
			switch(ECLevel)
			{
				case ErrorCorrectionLevel.L:
					return m_ECBlocks[0];
				case ErrorCorrectionLevel.M:
					return m_ECBlocks[1];
				case ErrorCorrectionLevel.Q:
					return m_ECBlocks[2];
				case ErrorCorrectionLevel.H:
					return m_ECBlocks[3];
				default:
					throw new System.ArgumentOutOfRangeException("Invalide ErrorCorrectionLevel");
			}
			
		}
		
	}
}
