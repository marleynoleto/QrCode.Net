using System;
using System.Collections.Generic;
using Gma.QrCodeNet.Encoding.ErrorCorrection;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ErrorCorrection
{
	[TestFixture]
	public class ECTest
	{
		[Test]
        [TestCaseSource(typeof(ECTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(IEnumerable<bool> dataCodewords, VersionCodewordsInfo vc, IEnumerable<bool> expected)
        {
        	BitList dcList = new BitList();
        	dcList.Add(dataCodewords);
        	
        	IEnumerable<bool> actualResult = ECGenerator.FillECCodewords(dcList, vc.NumTotalBytes, vc.NumDataBytes, vc.NumECBlocks);
        	string strResult = actualResult.To01String();
        	string strExpected = expected.To01String();
        	
        	if(!strResult.Equals(strExpected))
        		Assert.Fail("actual: {0} Expect: {1}", strResult, strExpected);
        }
	}
}
