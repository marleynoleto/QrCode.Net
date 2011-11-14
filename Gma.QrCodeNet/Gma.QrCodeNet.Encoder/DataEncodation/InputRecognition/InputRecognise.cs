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
		/// <exception cref="ArgumentException">If can not find proper Mode</exception>
		public static EncodationStruct Recognise(string content, string encodingName)
		{
			if(content == null || content == string.Empty)
				throw new ArgumentNullException();
			
			if(encodingName == null || encodingName == string.Empty)
				encodingName = QRCodeConstantVariable.DefaultEncoding;
			
			Mode mode = CheckALphaNumAndNum(content);
			
			if(mode != Mode.EightBitByte)
				return new EncodationStruct(mode, QRCodeConstantVariable.DefaultEncoding);
			
			if(ModeEncodeCheck.isModeEncodeValid(Mode.Kanji, encodingName, content))
				return new EncodationStruct(Mode.Kanji, "Shift_JIS");
			
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
		/// <exception cref="ArgumentException">If can not find proper Mode</exception>
		public static EncodationStruct Recognise(string content)
		{
			string encodingName = "";
			
			if(content == null || content.Length == 0)
				throw new ArgumentNullException();
			Mode mode = CheckALphaNumAndNum(content);
			
			if(mode != Mode.EightBitByte)
				return new EncodationStruct(mode, QRCodeConstantVariable.DefaultEncoding);
			
			if(ModeEncodeCheck.isModeEncodeValid(Mode.Kanji, "", content))
				return new EncodationStruct(Mode.Kanji, "Shift_JIS");
			
			Dictionary<string, int> eciSet = ECISet.GetECITable();
			
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
