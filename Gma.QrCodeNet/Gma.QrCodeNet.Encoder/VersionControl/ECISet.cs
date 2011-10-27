﻿using System.Collections.Generic;
using com.google.zxing.qrcode.encoder;

namespace Gma.QrCodeNet.Encoding.VersionControl
{
	internal sealed class ECISet
	{
		private static Dictionary<string, int> Name_To_Value;
		private static Dictionary<int, string> Value_To_Name;
		
		public enum AppendOption { NameToValue, ValueToName, Both }
		
		private static void AppendECI(string name, int value, AppendOption option)
		{
			switch(option)
			{
				case AppendOption.NameToValue:
					Name_To_Value.Add(name, value);
					break;
				case AppendOption.ValueToName:
					Value_To_Name.Add(value, name);
					break;
				case AppendOption.Both:
					Name_To_Value.Add(name, value);
					Value_To_Name.Add(value, name);
					break;
				default:
					throw new System.ArgumentOutOfRangeException("There is no such AppendOption");
			}
		}
		
		internal ECISet(AppendOption option)
		{
			Initialize(option);
		}
		
		private static void Initialize(AppendOption option)
		{
			switch(option)
			{
				case AppendOption.NameToValue:
					Name_To_Value = new Dictionary<string, int>();
					break;
				case AppendOption.ValueToName:
					Value_To_Name = new Dictionary<int, string>();
					break;
				case AppendOption.Both:
					Name_To_Value = new Dictionary<string, int>();
					Value_To_Name = new Dictionary<int, string>();
					break;
				default:
					throw new System.ArgumentOutOfRangeException("There is no such AppendOption");
			}
			
			//ECI table. Source 01 URL: http://strokescribe.com/en/ECI.html
			//ECI table. Source 02 URL: http://lab.must.or.kr/Extended-Channel-Interpretations-ECI-Encoding.ashx
			//ToDo. Fill up remaining missing table.
			AppendECI("iso-8859-1", 3, option);
			AppendECI("iso-8859-2", 4, option);
			AppendECI("iso-8859-3", 5, option);
			AppendECI("iso-8859-4", 6, option);
			AppendECI("iso-8859-5", 7, option);
			AppendECI("iso-8859-6", 8, option);
			AppendECI("iso-8859-7", 9, option);
			AppendECI("iso-8859-8", 10, option);
			AppendECI("iso-8859-9", 11, option);
			AppendECI("windows-874", 13, option);
			AppendECI("iso-8859-13", 15, option);
			AppendECI("iso-8859-15", 17, option);
			AppendECI("shift_jis", 20, option);
		}
		
		internal static int GetECIValueByName(string encodingName)
		{
			if(Name_To_Value == null)
				Initialize(AppendOption.NameToValue);
			int ECIValue;
			if(Name_To_Value.TryGetValue(encodingName, out ECIValue))
				return ECIValue;
			else
				throw new System.ArgumentOutOfRangeException(string.Format("ECI doesn't contain encoding: {0}", encodingName));
		}
		
		internal static string GetECINameByValue(int ECIValue)
		{
			if(Value_To_Name == null)
				Initialize(AppendOption.ValueToName);
			string ECIName;
			if(Value_To_Name.TryGetValue(ECIValue, out ECIName))
				return ECIName;
			else
				throw new System.ArgumentOutOfRangeException(string.Format("ECI doesn't contain value: {0}", ECIValue));
		}
		
		/// <remarks>ISO/IEC 18004:2006E ECI Designator Page 24</remarks>
		/// <param name="ECIValue">Range: 0 ~ 999999</param>
		/// <returns>Number of Codewords(Byte) for ECI Assignment Value</returns>
		private static int NumOfCodewords(int ECIValue)
		{
			if(ECIValue >= 0 && ECIValue <= 127)
				return 1;
			else if(ECIValue > 127 && ECIValue <= 16383)
				return 2;
			else if(ECIValue > 16383 && ECIValue <= 999999)
				return 3;
			else
				throw new System.ArgumentOutOfRangeException("ECIValue should be in range: 0 to 999999");
		}
		
		/// <remarks>ISO/IEC 18004:2006E ECI Designator Page 24</remarks>
		/// <param name="ECIValue">Range: 0 ~ 999999</param>
		/// <returns>Number of bits for ECI Assignment Value</returns>
		private static int NumOfAssignmentBits(int ECIValue)
		{
			return NumOfCodewords(ECIValue) * 8;
		}
		
		/// <remarks>ISO/IEC 18004:2006E ECI Designator Page 24</remarks>
		/// <param name="ECIValue">Range: 0 ~ 999999</param>
		/// <returns>Number of bits for ECI Header</returns>
		internal static int NumOfECIHeaderBits(int ECIValue)
		{
			return NumOfAssignmentBits(ECIValue) + 4;
		}
		
		/// <returns>ECI table in Dictionary collection</returns>
		internal static Dictionary<string, int> GetECITable()
		{
			if(Name_To_Value == null)
				Initialize(AppendOption.NameToValue);
			
			return Name_To_Value;
		}
		
		
		/// <summary>
		/// ISO/IEC 18004:2006 Chapter 6.4.2 Mode indicator = 0111 Page 23
		/// </summary>
		private const int ECIMode = 7;
		private const int ECIIndicatorNumBits = 4;
		
		/// <summary>
		/// Length indicator for number of ECI codewords
		/// </summary>
		/// <remarks>ISO/IEC 18004:2006 Chapter 6.4.2 Page 24.
		/// 1 codeword length = 0. Any additional codeword add 1 to front. Eg: 3 = 110</remarks>
		internal enum ECICodewordsLength { one = 0, two = 2, three = 6}
		
		/// <remarks>ISO/IEC 18004:2006 Chapter 6.4.2 Page 24.</remarks>
		internal static BitVector GetECIHeader(string encodingName)
		{
			int eciValue = GetECIValueByName(encodingName);
			
			BitVector dataBits = new BitVector();
			
			dataBits.appendBits(ECIMode, ECIIndicatorNumBits);
			
			int eciAssignmentByte = NumOfCodewords(eciValue);
			//Number of bits = Num codewords indicator + codeword value = Number of codewords * 8
			//Chapter 6.4.2.1 ECI Designator ISOIEC 18004:2006 Page 24
			int eciAssignmentBits = 0;	
			
			switch(eciAssignmentByte)
			{
				case 1:
					//Indicator = 0. Page 24. Chapter 6.4.2.1
					dataBits.appendBits((int)ECICodewordsLength.one, 1);
					eciAssignmentBits = eciAssignmentByte * 8 - 1;
					break;
				case 2:
					//Indicator = 10. Page 24. Chapter 6.4.2.1
					dataBits.appendBits((int)ECICodewordsLength.two, 2);
					eciAssignmentBits = eciAssignmentByte * 8 - 2;
					break;
				case 3:
					//Indicator = 110. Page 24. Chapter 6.4.2.1
					dataBits.appendBits((int)(int)ECICodewordsLength.three, 3);
					eciAssignmentBits = eciAssignmentByte * 8 - 3;
					break;
				default:
					throw new System.ArgumentOutOfRangeException("Assignment Codewords should be either 1, 2 or 3");
			}
			
			dataBits.appendBits(eciValue, eciAssignmentBits);
			
			return dataBits;
			
		}
		
	}
}