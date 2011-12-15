using System;
using Gma.QrCodeNet.Encoding.DataEncodation.InputRecognition;
using Gma.QrCodeNet.Encoding.Versions;
using Gma.QrCodeNet.Encoding.Terminate;

namespace Gma.QrCodeNet.Encoding.DataEncodation
{
	internal static class DataEncode
	{
		internal static EncodationStruct Encode(string content, ErrorCorrectionLevel ecLevel)
		{
			RecognitionStruct recognitionResult = InputRecognise.Recognise(content);
			EncoderBase encoderBase = CreateEncoder(recognitionResult.Mode, recognitionResult.EncodingName);
			
			BitList encodeContent = encoderBase.GetDataBits(content);
			
			int encodeContentLength = encodeContent.Count;
			
			VersionControlStruct vcStruct = 
				VersionControl.InitialSetup(encodeContentLength, recognitionResult.Mode, ecLevel, recognitionResult.EncodingName);
			
			BitList dataCodewords = new BitList();
			//Eci header
			if(vcStruct.isContainECI && vcStruct.ECIHeader != null)
				dataCodewords.Add(vcStruct.ECIHeader);
			//Header
			dataCodewords.Add(encoderBase.GetModeIndicator());
			int numLetter = recognitionResult.Mode == Mode.EightBitByte ? encodeContentLength >> 3 : content.Length;
			dataCodewords.Add(encoderBase.GetCharCountIndicator(numLetter, vcStruct.VersionDetail.Version));
			//Data
			dataCodewords.Add(encodeContent);
			//Terminator Padding
			dataCodewords.Add(Terminator.TerminateBites(dataCodewords.Count, vcStruct.VersionDetail.NumDataBytes));
			
			int dataCodewordsCount = dataCodewords.Count;
			if((dataCodewordsCount & 0x7) != 0)
				throw new ArgumentException("data codewords is not byte sized.");
			else if(dataCodewordsCount >> 3 != vcStruct.VersionDetail.NumDataBytes)
			{
				throw new ArgumentException("datacodewords num of bytes not equal to NumDataBytes for current version");
			}
			
			EncodationStruct encStruct = new EncodationStruct(vcStruct);
			encStruct.Mode = recognitionResult.Mode;
			encStruct.DataCodewords = dataCodewords;
			return encStruct;
		}
		
		
		private static EncoderBase CreateEncoder(Mode mode, string encodingName)
		{
			switch(mode)
			{
				case Mode.Numeric:
					return new NumericEncoder();
				case Mode.Alphanumeric:
					return new AlphanumericEncoder();
				case Mode.EightBitByte:
					return new EightBitByteEncoder(encodingName);
				case Mode.Kanji:
					return new KanjiEncoder();
				default:
					throw new ArgumentOutOfRangeException("mode", mode, string.Format("Doesn't contain encoder for {0}", mode));
			}
		}
	}
}
