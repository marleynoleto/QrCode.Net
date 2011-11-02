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
        	
        	VersionCheckStatus checkStatus = VersionTest.VersionCheck(qrCodeBox.Version, numDataBits, mode, level, encodingName);
        	
        	switch(checkStatus)
        	{
        		case VersionCheckStatus.LargerThanExpect:
        			Assert.Fail("Version {0} size not enough", qrCodeBox.Version);
        			break;
        		case VersionCheckStatus.SmallerThanExpect:
        			Assert.Fail("Version{0}'s size too big", qrCodeBox.Version);
        			break;
        		default:
        			break;
        	}
       
        }
        
        
        [Test]
        [TestCaseSource(typeof(VersionControlTestCaseFactory), "TestCasesFromCsvFile")]
        public void Test_against_CSV_Dataset(int numDataBits,  Mode mode, ErrorCorrectionLevel level, string encodingName, int expectVersionNum)
        {
        	QRCodeBox qrCodeBox = VersionControl.InitialSetup(numDataBits, mode, level, encodingName);
        	
        	if(qrCodeBox.Version != expectVersionNum)
        		Assert.Fail("Method return version number: {0} Expect value: {1}", qrCodeBox.Version, expectVersionNum);
        }
        
        //[Test]
        public void Generate()
        {
        	new VersionControlTestCaseFactory().GenerateTestDataSet();
        }
        
	}
}
