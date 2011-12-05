using Gma.QrCodeNet.Encoding.Versions;

namespace Gma.QrCodeNet.Encoding.DataEncodation
{
	internal struct EncodationStruct
	{
		internal int Version { get; set; }
		internal Mode Mode { get; set; }
		internal int MatrixWidth { get; set; }
		internal int NumTotalBytes { get; set; }
		internal int NumDataBytes { get; set; }
		internal int NumECBlocks { get; set; }
		internal BitList DataCodewords { get; set;}
		
		internal EncodationStruct(VersionControlStruct vcStruct)
			: this()
		{
			this.Version = vcStruct.Version;
			this.MatrixWidth = vcStruct.MatrixWidth;
			this.NumTotalBytes = vcStruct.NumTotalBytes;
			this.NumDataBytes = vcStruct.NumDataBytes;
			this.NumECBlocks = vcStruct.NumECBlocks;
		}
	}
}
