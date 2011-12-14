using System;

namespace Gma.QrCodeNet.Encoding.Tests.ErrorCorrection
{
	public struct VersionCodewordsInfo
	{
		public int NumTotalBytes { get; private set; }
		public int NumDataBytes { get; private set; }
		public int NumECBlocks { get; private set; }
		
		public VersionCodewordsInfo(int numTotalBytes, int numDataBytes, int numECBlocks)
			: this()
		{
			this.NumTotalBytes = numTotalBytes;
			this.NumDataBytes = numDataBytes;
			this.NumECBlocks = numECBlocks;
		}
		
		public VersionCodewordsInfo(string toString)
			: this()
		{
			string[] splitResult = toString.Split(new char[]{';'});
			if(splitResult.Length != 3)
				throw new ArgumentException("Given string does not contain int variable required by struct");
			NumTotalBytes = int.Parse(splitResult[0]);
			NumDataBytes = int.Parse(splitResult[1]);
			NumECBlocks = int.Parse(splitResult[2]);
		}
		
		public override string ToString()
		{
			return string.Join(";", NumTotalBytes, NumDataBytes, NumECBlocks);
		}
	}
}
