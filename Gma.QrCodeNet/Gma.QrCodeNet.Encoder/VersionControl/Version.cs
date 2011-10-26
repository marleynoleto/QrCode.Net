namespace Gma.QrCodeNet.Encoding.VersionControl
{
	internal struct Version
	{
		internal int VersionNum { get; private set;}
		
		internal int TotalCodewords { get; private set;}
		
		internal int DimensionForVersion { get; private set;}
		
		private ErrorCorrectionBlocks[] ecBlocks;
		
		/// <param name="ecblocks1">L</param>
		/// <param name="ecblocks2">M</param>
		/// <param name="ecblocks3">Q</param>
		/// <param name="ecblocks4">H</param>
		internal Version(int versionNum, int totalCodewords, ErrorCorrectionBlocks ecblocksL, ErrorCorrectionBlocks ecblocksM, ErrorCorrectionBlocks ecblocksQ, ErrorCorrectionBlocks ecblocksH)
			: this()
		{
			this.VersionNum = versionNum;
			this.TotalCodewords = totalCodewords;
			this.ecBlocks = new ErrorCorrectionBlocks[]{ecblocksL, ecblocksM, ecblocksQ, ecblocksH};
			this.DimensionForVersion = 17 + versionNum * 4;
		}
		
		/// <summary>
		/// Get Error Correction Blocks by level
		/// </summary>
		//[method
		internal ErrorCorrectionBlocks GetECBlocksByLevel(Gma.QrCodeNet.Encoding.ErrorCorrectionLevel ECLevel)
		{
			switch(ECLevel)
			{
				case ErrorCorrectionLevel.L:
					return ecBlocks[0];
				case ErrorCorrectionLevel.M:
					return ecBlocks[1];
				case ErrorCorrectionLevel.Q:
					return ecBlocks[2];
				case ErrorCorrectionLevel.H:
					return ecBlocks[3];
				default:
					throw new System.ArgumentOutOfRangeException("Invalide ErrorCorrectionLevel");
			}
			
		}
		
	}
}
