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
			int contentLength = content.Length;
			
			int tryEncodePos = ModeEncodeCheck.TryEncodeKanji(content, contentLength);
			
			if(tryEncodePos == -2)
				return new RecognitionStruct(Mode.EightBitByte, QRCodeConstantVariable.UTF8Encoding);
			else if(tryEncodePos == -1)
				return new RecognitionStruct(Mode.Kanji, QRCodeConstantVariable.DefaultEncoding);
			
			tryEncodePos = ModeEncodeCheck.TryEncodeAlphaNum(content, 0, contentLength);
			
			if(tryEncodePos == -2)
				return new RecognitionStruct(Mode.Numeric, QRCodeConstantVariable.DefaultEncoding);
			else if(tryEncodePos == -1)
				return new RecognitionStruct(Mode.Alphanumeric, QRCodeConstantVariable.DefaultEncoding);
			
			
			string encodingName = EightBitByteRecognision(content, tryEncodePos, contentLength);
			return new RecognitionStruct(Mode.EightBitByte, encodingName);
		}
		
		private static string EightBitByteRecognision(string content, int startPos, int contentLength)
		{
			if(string.IsNullOrEmpty(content))
				throw new ArgumentNullException("content", "Input content is null or empty");
			
			ECISet eciSets = new ECISet(ECISet.AppendOption.NameToValue);
			
			Dictionary<string, int> eciSet = eciSets.GetECITable();
			
			int scanPos = startPos;
			
			foreach(KeyValuePair<string, int> kvp in eciSet)
			{
				string encodingName = kvp.Key;
				if(encodingName != QRCodeConstantVariable.UTF8Encoding)
				{
					scanPos = ModeEncodeCheck.TryEncodeEightBitByte(content, encodingName, scanPos, contentLength);
					if(scanPos == -1)
					{
						int reScanPos = 0;
						reScanPos = ModeEncodeCheck.TryEncodeEightBitByte(content, encodingName, 0, contentLength);
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
		
	}
}
