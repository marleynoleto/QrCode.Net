using System;
using Gma.QrCodeNet.Encoding.DataEncodation;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests
{
	[TestFixture]
	public class ModeAutoSelectTest
	{
		
		[TestCase("15097435193480", (int)(Mode.Numeric))]
		[TestCase("35980SDLFKWETLKD", (int)(Mode.Alphanumeric))]
		[TestCase("r3546rraaaaÁÁÒÒÂÂöö", (int)(Mode.EightBitByte))]
		[TestCase("絞絞絞裁裁裁裁冊冊冊", (int)(Mode.Kanji))]
		[TestCase("123sad絞絞絞裁裁裁裁冊冊冊", -1)]
		public void AutoSelectTestOne(string content, int expectMode)
		{
			try
			{
				Mode mode = AutoSelectMode.AutoSelect(content, "");
				
				if(expectMode == -1)
					Assert.Fail("Mode return as {0}. But it should return as exception", mode);
				else if(mode != (Mode)expectMode)
					Assert.Fail("Mode return as {0}. But it should be {1}", mode, (Mode)expectMode);
			}catch
			{
				if(expectMode != -1)
					Assert.Fail("Mode should be = {0}. But it return as exception", (Mode)expectMode);
			}
			
		}
		
		
		
		
		[TestCase("15097435193480", "", (int)(Mode.Numeric))]
		[TestCase("35980SDLFKWETLKD", "", (int)(Mode.Alphanumeric))]
		[TestCase("r3546rraaaaÁÁÒÒÂÂöö", "iso-8859-1", (int)(Mode.EightBitByte))]
		[TestCase("r3546rraaaaｸｸｸｸﾗﾗﾗﾗｳｳｳ", "shift_jis", (int)(Mode.EightBitByte))]
		[TestCase("r3546rraaaaรรฝฝฝฝรสสสสสรร", "windows-874", (int)(Mode.EightBitByte))]
		[TestCase("絞絞絞裁裁裁裁冊冊冊", "", (int)(Mode.Kanji))]
		[TestCase("123sad絞絞絞裁裁裁裁冊冊冊", "", -1)]
		public void AutoSelectTestTwo(string content, string expectEncodingName, int expectMode)
		{
			try
			{
				string encodingName;
				Mode mode = AutoSelectMode.AutoSelect(content, out encodingName);
				
				if(expectMode == -1)
					Assert.Fail("Mode return as {0}. But it should return as exception", mode);
				else if(mode != (Mode)expectMode)
					Assert.Fail("Mode return as {0}. But it should be {1}", mode, (Mode)expectMode);
				else if(mode == (Mode)expectMode)
				{
					if(mode == Mode.EightBitByte)
					{
						if(encodingName != expectEncodingName)
							Assert.Fail("Encoding Name return as {0}. But it should be {1}", encodingName, expectEncodingName);
					}
				}
			}catch
			{
				if(expectMode != -1)
					Assert.Fail("Mode should be = {0}. But it return as exception", (Mode)expectMode);
			}
			
		}
	}
}
