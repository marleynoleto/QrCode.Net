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
        	
        	VersionControl.VersionCheckStatus checkStatus = VersionControl.VersionCheck(qrCodeBox.Version, numDataBits, mode, level, encodingName);
        	
        	switch(checkStatus)
        	{
        		case VersionControl.VersionCheckStatus.LargerThanExpect:
        			Assert.Fail("Version {0} size not enough", qrCodeBox.Version);
        			break;
        		case VersionControl.VersionCheckStatus.SmallerThanExpect:
        			Assert.Fail("Version{0}'s size too big", qrCodeBox.Version);
        			break;
        		default:
        			break;
        	}
       
        }
        
	}
}
