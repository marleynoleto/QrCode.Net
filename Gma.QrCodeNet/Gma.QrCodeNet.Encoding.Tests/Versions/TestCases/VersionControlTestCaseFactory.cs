using System;
using System.Collections.Generic;
using Gma.QrCodeNet.Encoding.DataEncodation;
using NUnit.Framework;


namespace Gma.QrCodeNet.Encoding.Tests.Versions.TestCases
{public class VersionControlTestCaseFactory
	{
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
		{
			//mode level bit length encoding name
			
			get
			{
				string[] encodingNames = new string[]{"iso-8859-1", "iso-8859-2"};
				Random randomizer = new Random();
				//Set to version 33. ECLevel = H
				int maxNumDataBits = (2611 - 1710) * 8;
				
				foreach(int modeValue in Enum.GetValues(typeof(Mode)))
				{
					foreach(int levelValue in Enum.GetValues(typeof(ErrorCorrectionLevel)))
					{
						foreach(string encodingName in encodingNames)
						{
							for(int i = 0; i < 15; i++)
							{
								int numDataBits = randomizer.Next(1, maxNumDataBits);
								
								yield return new TestCaseData(numDataBits, (Mode)modeValue, (ErrorCorrectionLevel)levelValue, encodingName);
							}
						}
					}
				}
			}
		}
		
	}
}
