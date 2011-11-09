using System;
using System.Text;

namespace Gma.QrCodeNet.Encoding.DataEncodation
{
	public static class ModeEncodeCheck
	{
		private const string DEFAULT_ENCODING = "iso-8859-1";
		
		public static bool isModeEncodeValid(Mode mode, string encoding, string content)
		{
			switch(mode)
			{
				case Mode.Numeric:
					return NumericCheck(content);
				case Mode.Alphanumeric:
					return AlphaNumCheck(content);
				case Mode.EightBitByte:
					return EightBitByteCheck(encoding, content);
				case Mode.Kanji:
					return KanjiCheck(content);
				default:
					throw new InvalidOperationException(string.Format("System does not contain mode: {0}", mode.ToString()));
			}
		}
		
		private static bool NumericCheck(string content)
		{
			int num = new int();
			for(int index = 0; index < content.Length; index++)
			{
				num = content[index] - '0';
				if(num < 0 || num > 9)
					return false;
			}
			
			return true;
		}
		
		
		private static bool AlphaNumCheck(string content)
		{
			for(int index = 0; index < content.Length; index++)
			{
				if(!AlphanumericTable.Contains(content[index]))
				{
					return false;
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// Encoding.GetEncoding.GetBytes will transform char to 0x3F if that char not belong to current encoding table. 
		/// 0x3F is '?'
		/// </summary>
		private const int QUESTION_MARK_CHAR = 0x3F;
		
		private static bool EightBitByteCheck(string encodingName, string content)
		{
			System.Text.Encoding encoding;
			try
			{
				encoding = System.Text.Encoding.GetEncoding(encodingName);
			} catch
			{
				return false;
			}
			
			char[] currentChar = new char[1];
			byte[] bytes;
			
			
			for(int index = 0; index < content.Length; index++)
			{
				currentChar[0] = content[index];
				bytes = encoding.GetBytes(currentChar);
				int length = bytes.Length;
				if(currentChar[0] != '?' && length == 1 && (int)bytes[0] == QUESTION_MARK_CHAR)
					return false;
				else if(length > 1)
					return false;
			}
			
			return true;
		}
		
		private static bool KanjiCheck(string content)
		{
			byte[] bytes = System.Text.Encoding.GetEncoding("Shift_JIS").GetBytes(content);
			int bytesLength = bytes.Length;
			
			if(bytesLength / 2 == content.Length)
			{
				for(int index = 0; index < bytesLength; index += 2)
				{
					byte mostSignificantByte = bytes[index];
					if ((mostSignificantByte < 0x81 || mostSignificantByte > 0x9F) && (mostSignificantByte < 0xE0 || mostSignificantByte > 0xEB))
					{
						return false;
					}
				}
			}
			else
				return false;
			
			return true;
		}
	}
}
