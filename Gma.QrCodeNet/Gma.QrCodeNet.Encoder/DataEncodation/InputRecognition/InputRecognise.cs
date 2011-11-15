using System;
using System.Collections.Generic;
using Gma.QrCodeNet.Encoding.Versions;

namespace Gma.QrCodeNet.Encoding.DataEncodation.InputRecognition
{
	public static class InputRecognise
	{
		
		/// <summary>
		/// Common method to detect Mode.  
		/// </summary>
		/// <param name="content">input string content</param>
		/// <param name="encodingName">Encoding name 
		/// (It will use iso-8859-1 as default encoding name if variable is null or empty string)</param>
		/// <returns>Mode that will be use to encode content</returns>
		public static EncodationStruct Recognise(string content, string encodingName)
		{
			
			if(encodingName == null || encodingName == string.Empty)
				encodingName = QRCodeConstantVariable.DefaultEncoding;
			
			Mode mode = CheckModeOtherThanEightBitByte(content);
			
			if(mode != Mode.EightBitByte)
				return new EncodationStruct(mode, QRCodeConstantVariable.DefaultEncoding);
			
			if(ModeEncodeCheck.isModeEncodeValid(Mode.EightBitByte, encodingName, content))
				return new EncodationStruct(Mode.EightBitByte, encodingName);
			
			
			return new EncodationStruct(Mode.EightBitByte, QRCodeConstantVariable.UTF8Encoding);
			
		}
		
		/// <summary>
		/// Cost most resource. Only use if you don't know what encoding table client will be using. 
		/// </summary>
		/// <param name="content">input string content</param>
		/// <param name="encodingName">Output encoding name</param>
		/// <returns>Mode that will be use to encode content</returns>
		public static EncodationStruct Recognise(string content)
		{
			
			
			Mode mode = CheckModeOtherThanEightBitByte(content);
			
			if(mode != Mode.EightBitByte)
				return new EncodationStruct(mode, QRCodeConstantVariable.DefaultEncoding);
			
			Dictionary<string, int> eciSet = ECISet.GetECITable();
			
			string encodingName = "";
			
			foreach(KeyValuePair<string, int> kvp in eciSet)
			{
				if(kvp.Key != QRCodeConstantVariable.UTF8Encoding)
				{
					if(ModeEncodeCheck.isModeEncodeValid(Mode.EightBitByte, kvp.Key, content))
					{
						encodingName = kvp.Key;
						return new EncodationStruct(Mode.EightBitByte, encodingName);
					}
				}
			}
			
			return new EncodationStruct(Mode.EightBitByte, QRCodeConstantVariable.UTF8Encoding);
			
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
