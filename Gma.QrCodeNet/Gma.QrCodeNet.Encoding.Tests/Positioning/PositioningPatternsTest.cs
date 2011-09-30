using Gma.QrCodeNet.Encoding.Masking;
using Gma.QrCodeNet.Encoding.Tests.Masking;
using Gma.QrCodeNet.Encoding.Tests.PositionAdjustment.TestCases;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.PositionAdjustment
{
    [TestFixture]
    public class PositioningPatternsTest
    {

        //[Test]
        //[TestCaseSource(typeof(MaskPatternTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(BitMatrix input, MaskPatternType patternType, BitMatrix expected)
        {
            Pattern pattern = new PatternFactory().CreateByType(patternType);

            BitMatrix result = input.Apply(pattern);

            expected.AssertEquals(result);
        }

        //[Test]
        //[TestCaseSource(typeof(MaskPatternTestCaseFactory), "TestCasesFromTxtFile")]
        public void Test_against_DataSet(BitMatrix input, MaskPatternType patternType, BitMatrix expected)
        {
            Pattern pattern = new PatternFactory().CreateByType(patternType);

            BitMatrix result = input.Apply(pattern);

            expected.AssertEquals(result);
        }

        [Test]
        public void Generate()
        {
            new PositioningPatternsTestCaseFactory().GenerateMaskPatternTestDataSet();
        }
    }
}
