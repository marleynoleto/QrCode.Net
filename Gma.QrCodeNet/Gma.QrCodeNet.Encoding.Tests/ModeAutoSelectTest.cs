using System;
using Gma.QrCodeNet.Encoding.DataEncodation;
using NUnit.Framework;
using zxMode = com.google.zxing.qrcode.decoder.Mode;

namespace Gma.QrCodeNet.Encoding.Tests
{
	[TestFixture]
	public class ModeAutoSelectTest
	{
		
		[TestCase("15097435193480", (int)(Mode.Numeric))]
		[TestCase("35980SDLFKWETLKD", (int)(Mode.Alphanumeric))]
		[TestCase("r3546rraaaaÁÁÒÒÂÂöö", (int)(Mode.EightBitByte))]
		[TestCase("絞絞絞裁裁裁裁冊冊冊", (int)(Mode.Kanji))]
		[TestCase("123sad絞絞絞裁裁裁裁冊冊冊", (int)(Mode.None))]
		public void AutoSelectTestOne(string content, int expectMode)
		{
			
			Mode mode;
			
			mode = AutoSelectMode.AutoSelect(content, "");
			if(mode != (Mode)expectMode)
				Assert.Fail("Mode return as {0}. But it should be {1}", mode, (Mode)expectMode);
			
		}
		
		
		
		
		[TestCase("15097435193480", "", (int)(Mode.Numeric))]
		[TestCase("35980SDLFKWETLKD", "", (int)(Mode.Alphanumeric))]
		[TestCase("r3546rraaaaÁÁÒÒÂÂöö", "iso-8859-1", (int)(Mode.EightBitByte))]
		[TestCase("r3546rraaaaｸｸｸｸﾗﾗﾗﾗｳｳｳ", "shift_jis", (int)(Mode.EightBitByte))]
		[TestCase("r3546rraaaaรรฝฝฝฝรสสสสสรร", "windows-874", (int)(Mode.EightBitByte))]
		[TestCase("絞絞絞裁裁裁裁冊冊冊", "", (int)(Mode.Kanji))]
		[TestCase("123sad絞絞絞裁裁裁裁冊冊冊", "", (int)(Mode.None))]
		public void AutoSelectTestTwo(string content, string expectEncodingName, int expectMode)
		{
			string encodingName;
			Mode mode = AutoSelectMode.AutoSelect(content, out encodingName);
				
			if(mode != (Mode)expectMode)
				Assert.Fail("Mode return as {0}. But it should be {1}", mode, (Mode)expectMode);
			else if(mode == (Mode)expectMode)
			{
				if(mode == Mode.EightBitByte)
				{
					if(encodingName != expectEncodingName)
							Assert.Fail("Encoding Name return as {0}. But it should be {1}", encodingName, expectEncodingName);
				}
			}
			
			
		}
		
		
		[TestCase("15097435193480", "iso-8859-1", (int)(Mode.Numeric))]
		[TestCase("35980SDLFKWETLKD", "iso-8859-1", (int)(Mode.Alphanumeric))]
		[TestCase("r3546rraaaaÁÁÒÒÂÂöö", "iso-8859-1", (int)(Mode.EightBitByte))]
		[TestCase("r3546rraaaaｸｸｸｸﾗﾗﾗﾗｳｳｳ", "iso-8859-1", -1)]
		[TestCase("絞絞絞裁裁裁裁冊冊冊", "Shift_JIS", (int)(Mode.Kanji))]
		[TestCase("123sad絞絞絞裁裁裁裁冊冊冊", "Shift_JIS", (int)(Mode.None))]
		public void AutoSelectTestZX(string content, string encodingName, int expectMode)
		{
			zxMode mode = null;
			
			mode = ZXchooseMode(content, encodingName);
		
			if(!isModeEqual((Mode)expectMode, mode))
					Assert.Fail("Mode return as {0}. But it should be {1}", mode, (Mode)expectMode);
			
		}
		
		private bool isModeEqual(Mode mode, zxMode zxmode)
		{
			switch(mode)
			{
				case Mode.Numeric:
					return zxmode == zxMode.NUMERIC ? true : false;
				case Mode.Alphanumeric:
					return zxmode == zxMode.ALPHANUMERIC ? true : false;
				case Mode.EightBitByte:
					return zxmode == zxMode.BYTE ? true : false;
				case Mode.Kanji:
					return zxmode == zxMode.KANJI ? true : false;
				default:
					return false;
			}
		}
		
		
		private static readonly int[] ALPHANUMERIC_TABLE = new int[]{- 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, - 1, 36, - 1, - 1, - 1, 37, 38, - 1, - 1, - 1, - 1, 39, 40, - 1, 41, 42, 43, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 44, - 1, - 1, - 1, - 1, - 1, - 1, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, - 1, - 1, - 1, - 1, - 1};
		
		
		internal static zxMode ZXchooseMode(System.String content, System.String encoding)
		{
			if ("Shift_JIS".Equals(encoding))
			{
				// Choose Kanji mode if all input are double-byte characters
				return isOnlyDoubleByteKanji(content)?zxMode.KANJI:zxMode.BYTE;
			}
			bool hasNumeric = false;
			bool hasAlphanumeric = false;
			for (int i = 0; i < content.Length; ++i)
			{
				char c = content[i];
				if (c >= '0' && c <= '9')
				{
					hasNumeric = true;
				}
				else if (getAlphanumericCode(c) != - 1)
				{
					hasAlphanumeric = true;
				}
				else
				{
					return zxMode.BYTE;
				}
			}
			if (hasAlphanumeric)
			{
				return zxMode.ALPHANUMERIC;
			}
			else if (hasNumeric)
			{
				return zxMode.NUMERIC;
			}
			return zxMode.BYTE;
		}
		
		internal static int getAlphanumericCode(int code)
		{
			if (code < ALPHANUMERIC_TABLE.Length)
			{
				return ALPHANUMERIC_TABLE[code];
			}
			return - 1;
		}
		
		private static bool isOnlyDoubleByteKanji(System.String content)
		{
			sbyte[] bytes;
			try
			{
				//UPGRADE_TODO: Method 'java.lang.String.getBytes' was converted to 'System.Text.Encoding.GetEncoding(string).GetBytes(string)' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangStringgetBytes_javalangString'"
				bytes = SupportClass.ToSByteArray(System.Text.Encoding.GetEncoding("Shift_JIS").GetBytes(content));
			}
			catch (System.IO.IOException)
			{
				return false;
			}
			int length = bytes.Length;
			if (length % 2 != 0)
			{
				return false;
			}
			for (int i = 0; i < length; i += 2)
			{
				int byte1 = bytes[i] & 0xFF;
				if ((byte1 < 0x81 || byte1 > 0x9F) && (byte1 < 0xE0 || byte1 > 0xEB))
				{
					return false;
				}
			}
			return true;
		}
	}
}
