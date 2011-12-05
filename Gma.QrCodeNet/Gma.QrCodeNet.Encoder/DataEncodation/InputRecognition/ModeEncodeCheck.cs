using System;
using System.Text;

namespace Gma.QrCodeNet.Encoding.DataEncodation.InputRecognition
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
			int tryEncodePos = TryEncodeEightBitByte(content, encodingName, 0);
			return tryEncodePos == -1 ? true : false;
		}
		
		/// <summary>
		/// Use given encoding to check input string from starting position. If encoding table is suitable solution.
		/// it will return -1. Else it will return failed encoding position. 
		/// </summary>
		/// <param name="content">input string</param>
		/// <param name="encodingName">encoding name. Check ECI table</param>
		/// <param name="startPos">starting position</param>
		/// <returns>-1 if from starting position to end encoding success. Else return fail position</returns>
		internal static int TryEncodeEightBitByte(string content, string encodingName, int startPos)
		{
			int contentLength = content.Length;
			if(startPos >= contentLength)
				throw new IndexOutOfRangeException("startPos greater or equal to content length");
			
			System.Text.Encoding encoding;
			try
			{
				encoding = System.Text.Encoding.GetEncoding(encodingName);
			} catch(ArgumentException)
			{
				return startPos;
			}
			
			char[] currentChar = new char[1];
			byte[] bytes;
			
			
			for(int index = startPos; index < contentLength; index++)
			{
				currentChar[0] = content[index];
				bytes = encoding.GetBytes(currentChar);
				int length = bytes.Length;
				if(currentChar[0] != '?' && length == 1 && (int)bytes[0] == QUESTION_MARK_CHAR)
					return index;
				else if(length > 1)
					return index;
			}
			
			return -1;
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
