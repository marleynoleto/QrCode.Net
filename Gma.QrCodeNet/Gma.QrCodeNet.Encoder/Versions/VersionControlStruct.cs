namespace Gma.QrCodeNet.Encoding.Versions
{
	internal struct VersionControlStruct
	{
		internal int Version { get; set; }
		internal int MatrixWidth { get; set; }
		internal int NumTotalBytes { get; set; }
		internal int NumDataBytes { get; set; }
		internal int NumECBlocks { get; set; }
		internal bool isContainECI { get; set; }
		internal BitList ECIHeader { get; set; }
	}
}
