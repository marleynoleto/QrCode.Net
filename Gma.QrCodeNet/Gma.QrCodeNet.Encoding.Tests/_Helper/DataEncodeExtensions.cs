using System;
using System.Collections.Generic;
using com.google.zxing.qrcode.decoder;
using com.google.zxing.qrcode.encoder;
using com.google.zxing.common;
using Gma.QrCodeNet.Encoding.DataEncodation;
using Gma.QrCodeNet.Encoding.Versions;
using Gma.QrCodeNet.Encoding.DataEncodation.InputRecognition;
using GMode = Gma.QrCodeNet.Encoding.DataEncodation.Mode;
using Mode = com.google.zxing.qrcode.decoder.Mode;


namespace Gma.QrCodeNet.Encoding.Tests._Helper
{
	public static class DataEncodeExtensions
	{
		/// <summary>
        /// Combine Gma.QrCodeNet.Encoding input recognition method and version control method
        /// with legacy code. To create expected answer. 
        /// This is base on assume Gma.QrCodeNet.Encoding input recognition and version control sometime
        /// give different result as legacy code. 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static IEnumerable<bool> DataEncodeUsingReferenceImplementation(string content)
        {
        	//Choose mode
        	RecognitionStruct recognitionResult = InputRecognise.Recognise(content);
        	string encodingName = recognitionResult.EncodingName;
        	Mode mode = ConvertMode(recognitionResult.Mode);
        	
        	//append byte to databits
        	BitVector dataBits = new BitVector();
			EncoderInternal.appendBytes(content, mode, dataBits, encodingName);
			
			int dataBitsLength = dataBits.size();
			VersionControlStruct vcStruct = 
				VersionControl.InitialSetup(dataBitsLength, recognitionResult.Mode, ErrorCorrectionLevel.H, recognitionResult.EncodingName);
			//ECI
			BitVector headerAndDataBits = new BitVector();
			string defaultByteMode = "iso-8859-1";
			if (mode == Mode.BYTE && !defaultByteMode.Equals(encodingName))
			{
				CharacterSetECI eci = CharacterSetECI.getCharacterSetECIByName(encodingName);
				if (eci != null)
				{
					EncoderInternal.appendECI(eci, headerAndDataBits);
				}
			}
			//Mode
			EncoderInternal.appendModeInfo(mode, headerAndDataBits);
			//Char info
			int numLetters = mode.Equals(Mode.BYTE)?dataBits.sizeInBytes():content.Length;
			EncoderInternal.appendLengthInfo(numLetters, vcStruct.Version, mode, headerAndDataBits);
			//Combine with dataBits
			headerAndDataBits.appendBitVector(dataBits);
			
			// Terminate the bits properly.
			EncoderInternal.terminateBits(vcStruct.NumDataBytes, headerAndDataBits);
			
			return headerAndDataBits;
        }
        
        private static Mode ConvertMode(GMode mode)
        {
        	switch(mode)
        	{
        		case GMode.Numeric:
        			return Mode.NUMERIC;
        		case GMode.Alphanumeric:
        			return Mode.ALPHANUMERIC;
        		case GMode.EightBitByte:
        			return Mode.BYTE;
        		case GMode.Kanji:
        			return Mode.KANJI;
        		default:
        			throw new ArgumentOutOfRangeException("mode", mode, string.Format("Gma mode doesn't contain {0}", mode));
        	}
        }
	}
}
