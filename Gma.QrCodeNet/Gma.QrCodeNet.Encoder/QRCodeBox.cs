using Gma.QrCodeNet.Encoding.DataEncodation;

namespace Gma.QrCodeNet.Encoding
{
	public sealed class QRCodeBox
	{
		private Mode m_mode;
		
		internal Mode Mode 
		{
			get
			{
				return m_mode;
			}
			set
			{
				m_mode = value;
			}
		}
		
		private ErrorCorrectionLevel m_errorCorrectionLevel;
		
		internal ErrorCorrectionLevel ErrorCorrectionLevel
		{
			get
			{
				return m_errorCorrectionLevel;
			}
			set
			{
				m_errorCorrectionLevel = value;
			}
		}
		
		private int m_version;
		
		public int Version
		{
			get
			{
				return m_version;
			}
			set
			{
				m_version = value;
			}
		}
		
		private int m_matrixWidth;
		
		
		internal int MatrixWidth
		{
			get
			{
				return m_matrixWidth;
			}
			set
			{
				m_matrixWidth = value;
			}
		}
		
		private Masking.MaskPatternType m_maskPattern;
		
		internal Masking.MaskPatternType MaskPattern
		{
			get
			{
				return m_maskPattern;
			}
			set
			{
				m_maskPattern = value;
			}
		}
		
		private int m_numTotalBytes;
		
		internal int NumTotalBytes
		{
			get
			{
				return m_numTotalBytes;
			}
			set
			{
				m_numTotalBytes = value;
			}
		}
		
		private int m_numDataBytes;
		
		internal int NumDataBytes
		{
			get
			{
				return m_numDataBytes;
			}
			set
			{
				m_numDataBytes = value;
			}
		}
		
		
		private int m_numErrorCorrectionBytes;
		
		internal int NumErrorCorrectionBytes
		{
			get
			{
				return m_numErrorCorrectionBytes;
			}
			set
			{
				m_numErrorCorrectionBytes = value;
			}
		}
		
		private int m_numErrorCorrectionBlocks;
		
		internal int NumErrorCorrectionBlocks
		{
			get
			{
				return m_numErrorCorrectionBlocks;
			}
			set
			{
				m_numErrorCorrectionBlocks = value;
			}
		}
		
		private BitList m_eciDataBits;
		
		internal BitList ECIDataBits
		{
			get
			{
				return m_eciDataBits;
			}
			set
			{
				m_eciDataBits = value;
			}
		}
		
		private BitList m_headerDataBits;
		
		internal BitList HeaderDataBits
		{
			get
			{
				return m_headerDataBits;
			}
			set
			{
				m_headerDataBits = value;
			}
		}
		
	}
}
