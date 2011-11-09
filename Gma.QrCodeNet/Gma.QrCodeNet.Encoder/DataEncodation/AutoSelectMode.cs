using System;
using System.Collections.Generic;
using Gma.QrCodeNet.Encoding.Versions;

namespace Gma.QrCodeNet.Encoding.DataEncodation
{
	public static class AutoSelectMode
	{
		
		/// <summary>
		/// Common method to detect Mode.  
		/// </summary>
		/// <param name="content">input string content</param>
		/// <param name="encodingName">Encoding name 
		/// (It will use iso-8859-1 as default encoding name if variable is null or empty string)</param>
		/// <returns>Mode that will be use to encode content</returns>
		/// <exception cref="ArgumentException">If can not find proper Mode</exception>
		public static Mode AutoSelect(string content, string encodingName)
		{
			if(content == null || content == string.Empty)
				throw new ArgumentNullException();
			
			if(encodingName == null || encodingName == string.Empty)
				encodingName = "iso-8859-1";
			
			Mode mode = CheckALphaNumAndNum(content);
			
			if(mode != Mode.EightBitByte)
				return mode;
			
			if(ModeEncodeCheck.isModeEncodeValid(Mode.EightBitByte, encodingName, content))
				return Mode.EightBitByte;
			
			if(ModeEncodeCheck.isModeEncodeValid(Mode.Kanji, encodingName, content))
				return Mode.Kanji;
			
			return Mode.None;
			
		}
		
		/// <summary>
		/// Cost most resource. Only use if you don't know what encoding table client will be using. 
		/// </summary>
		/// <param name="content">input string content</param>
		/// <param name="encodingName">Output encoding name</param>
		/// <returns>Mode that will be use to encode content</returns>
		/// <exception cref="ArgumentException">If can not find proper Mode</exception>
		public static Mode AutoSelect(string content, out string encodingName)
		{
			encodingName = "";
			
			if(content == null || content.Length == 0)
				throw new ArgumentNullException();
			Mode mode = CheckALphaNumAndNum(content);
			
			if(mode != Mode.EightBitByte)
				return mode;
			
			if(ModeEncodeCheck.isModeEncodeValid(Mode.Kanji, "", content))
				return Mode.Kanji;
			
			Dictionary<string, int> eciSet = ECISet.GetECITable();
			
			foreach(KeyValuePair<string, int> kvp in eciSet)
			{
				if(ModeEncodeCheck.isModeEncodeValid(Mode.EightBitByte, kvp.Key, content))
				{
					encodingName = kvp.Key;
					return Mode.EightBitByte;
				}
			}
			
			return Mode.None;
			
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
