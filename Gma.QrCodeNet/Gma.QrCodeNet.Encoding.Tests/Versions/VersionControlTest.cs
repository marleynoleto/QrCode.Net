using NUnit.Framework;
using Gma.QrCodeNet.Encoding.Tests.Versions.TestCases;
using Gma.QrCodeNet.Encoding.DataEncodation;
using Gma.QrCodeNet.Encoding.Versions;

namespace Gma.QrCodeNet.Encoding.Tests.Versions
{
	[TestFixture]
	public class VersionControlTest
	{
		private static VersionTable versionTable = new VersionTable();
		
		[Test]
        [TestCaseSource(typeof(VersionControlTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int numDataBits,  Mode mode, ErrorCorrectionLevel level, string encodingName)
        {
        	QRCodeBox qrCodeBox = VersionControl.InitialSetup(numDataBits, mode, level, encodingName);
        	
        	int TotalDataBits = numDataBits;
        	if(mode == Mode.EightBitByte)
        	{
        		if(encodingName != "iso-8859-1")
        		{
        			TotalDataBits += 4;
        			TotalDataBits += 8;
        		}
        	}
        	int[] charCountIndicator = CharCountIndicatorTable.GetCharCountIndicator(mode);
        	TotalDataBits += (4 + charCountIndicator[GetVersionGroup(qrCodeBox.Version)]);
        	
        	int expectContainer = DataBits(qrCodeBox.Version, level);
        	int lowerContainer = qrCodeBox.Version == 1 ? 0 : DataBits(qrCodeBox.Version - 1, level);
        	
        	if(expectContainer < TotalDataBits)
        	{
        		Assert.Fail("Version {0} can not contain {1} data bits", qrCodeBox.Version, TotalDataBits);
        	}
        	
        	if(lowerContainer >= TotalDataBits)
        	{
        		Assert.Fail("Version{0}'s one level lower can contain {1} data bits", qrCodeBox.Version, TotalDataBits);
        	}
       
        }
        
        protected int GetVersionGroup(int version)
        {
        	if (version > 40)
        	{
        		throw new System.InvalidOperationException(string.Format("Unexpected version: {0}", version));
        	}
            else if (version>= 27)
            {
                return 2;
            }
			else if (version >= 10)
            {
                return 1;
            }
			else if (version > 0)
			{
				return 0;
			}
			else
				throw new System.InvalidOperationException(string.Format("Unexpected version: {0}", version));

        }
        
        private int DataBits(int version, ErrorCorrectionLevel level)
        {
        	int totalCodewords = versionTable.GetVersionByNum(version).TotalCodewords;
        	int totalECCodewords = versionTable.GetVersionByNum(version).GetECBlocksByLevel(level).NumErrorCorrectionCodewards;
        	
        	return (totalCodewords - totalECCodewords) * 8;
        }
	}
}
