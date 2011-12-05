using System;
using System.Collections.Generic;
using Gma.QrCodeNet.Encoding.Versions;

namespace Gma.QrCodeNet.Encoding.DataEncodation.InputRecognition
{
	public static class InputRecognise
	{
		
		/// <summary>
		/// Use to recognise which mode and encoding name to use for input string. 
		/// </summary>
		/// <param name="content">input string content</param>
		/// <param name="encodingName">Output encoding name</param>
		/// <returns>Mode and Encoding name</returns>
		public static RecognitionStruct Recognise(string content)
		{
			
			
			Mode mode = CheckModeOtherThanEightBitByte(content);
			
			if(mode != Mode.EightBitByte)
				return new RecognitionStruct(mode, QRCodeConstantVariable.DefaultEncoding);
			
			string encodingName = EightBitByteRecognision(content);
			return new RecognitionStruct(Mode.EightBitByte, encodingName);
		}
		
		private static string EightBitByteRecognision(string content)
		{
			if(string.IsNullOrEmpty(content))
				throw new ArgumentNullException("content", "Input content is null or empty");
			
			ECISet eciSets = new ECISet(ECISet.AppendOption.NameToValue);
			
			Dictionary<string, int> eciSet = eciSets.GetECITable();
			
			int scanPos = 0;
			
			foreach(KeyValuePair<string, int> kvp in eciSet)
			{
				string encodingName = kvp.Key;
				if(encodingName != QRCodeConstantVariable.UTF8Encoding)
				{
					scanPos = ModeEncodeCheck.TryEncodeEightBitByte(content, encodingName, scanPos);
					if(scanPos == -1)
					{
						int reScanPos = 0;
						reScanPos = ModeEncodeCheck.TryEncodeEightBitByte(content, encodingName, 0);
						if(reScanPos == -1)
						{
							return encodingName;
						}
						else
							scanPos = reScanPos;
					}
				}
			}
			
			if(scanPos == -1)
				throw new ArgumentException("foreach Loop check give wrong result.");
			else
				return QRCodeConstantVariable.UTF8Encoding;
			
		}
		
		
		
		private static Mode CheckModeOtherThanEightBitByte(string content)
		{
			if(content == null || content.Length == 0)
				throw new ArgumentNullException();
			Mode mode = CheckALphaNumAndNum(content);
			
			if(mode != Mode.EightBitByte)
				return mode;
			
			if(ModeEncodeCheck.isModeEncodeValid(Mode.Kanji, "", content))
				return Mode.Kanji;
			
			return Mode.EightBitByte;
		}
		
		private static Mode CheckALphaNumAndNum(string content)
		{
			bool isNumeric = true;
			bool isAlphaNum = true;
			
			for(int index = 0; index < content.Length; index++)
			{
				char CurrentChar = content[index];
				if(CurrentChar < '0' || CurrentChar > '9')
				{
					isNumeric = false;
				}
				if(!AlphanumericTable.Contains(CurrentChar))
				{
					isAlphaNum = false;
				}
			}
			
			if(isNumeric)
				return Mode.Numeric;
			if(isAlphaNum)
				return Mode.Alphanumeric;
			
			return Mode.EightBitByte;
		}
		
		
		
	}
}
