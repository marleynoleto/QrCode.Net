using System;
using Gma.QrCodeNet.Encoding.DataEncodation;
using Gma.QrCodeNet.Encoding.DataEncodation.InputRecognition;
using NUnit.Framework;
using zxMode = com.google.zxing.qrcode.decoder.Mode;

namespace Gma.QrCodeNet.Encoding.Tests
{
	[TestFixture]
	public class ModeAutoSelectTest
	{
		
		[TestCase("15097435193480", (int)(Mode.Numeric), "")]
		[TestCase("35980SDLFKWETLKD", (int)(Mode.Alphanumeric), "")]
		[TestCase("r3546rraaaaÁÁÒÒÂÂöö", (int)(Mode.EightBitByte), QRCodeConstantVariable.DefaultEncoding)]
		[TestCase("絞絞絞裁裁裁裁冊冊冊", (int)(Mode.Kanji), "")]
		[TestCase("123sad絞絞絞裁裁裁裁冊冊冊", (int)(Mode.EightBitByte), QRCodeConstantVariable.UTF8Encoding)]
		[TestCase("ｦｦｦｦｦﾝﾝﾝﾝﾝ絞絞絞裁裁裁裁冊冊冊", (int)(Mode.EightBitByte), QRCodeConstantVariable.UTF8Encoding)]
		public void AutoSelectTestOne(string content, int expectMode, string expectEncodingName)
		{
			
			EncodationStruct structValue;
			
			structValue = InputRecognise.Recognise(content, "");
			if(structValue.Mode != (Mode)expectMode)
				Assert.Fail("Mode return as {0}. But it should be {1}", structValue.Mode, (Mode)expectMode);
			
			if(structValue.Mode == Mode.EightBitByte && structValue.EncodingName != expectEncodingName)
				Assert.Fail("Encoding Name return as {0}. But it should be {1}", structValue.EncodingName, expectEncodingName);
			
		}
		
		
		
		
		[TestCase("15097435193480", "", (int)(Mode.Numeric))]
		[TestCase("35980SDLFKWETLKD", "", (int)(Mode.Alphanumeric))]
		[TestCase("r3546rraaaaÁÁÒÒÂÂöö", QRCodeConstantVariable.DefaultEncoding, (int)(Mode.EightBitByte))]
		[TestCase("r3546rraaaaｸｸｸｸﾗﾗﾗﾗｳｳｳ", "shift_jis", (int)(Mode.EightBitByte))]
		[TestCase("r3546rraaaaรรฝฝฝฝรสสสสสรร", "windows-874", (int)(Mode.EightBitByte))]
		[TestCase("ｦｦｦｦｦﾝﾝﾝﾝﾝ絞絞絞裁裁裁裁冊冊冊", QRCodeConstantVariable.UTF8Encoding, (int)(Mode.EightBitByte))]
		[TestCase("絞絞絞裁裁裁裁冊冊冊", "", (int)(Mode.Kanji))]
		[TestCase("123sad絞絞絞裁裁裁裁冊冊冊", QRCodeConstantVariable.UTF8Encoding, (int)(Mode.EightBitByte))]
		public void AutoSelectTestTwo(string content, string expectEncodingName, int expectMode)
		{
			EncodationStruct structValue = InputRecognise.Recognise(content);
				
			if(structValue.Mode != (Mode)expectMode)
				Assert.Fail("Mode return as {0}. But it should be {1}", structValue.Mode, (Mode)expectMode);
			
			if(structValue.Mode == Mode.EightBitByte)
			{
				if(structValue.EncodingName != expectEncodingName)
						Assert.Fail("Encoding Name return as {0}. But it should be {1}", structValue.EncodingName, expectEncodingName);
			}
			
			
			
		}
		
		
	}
}
