using System.Collections.Generic;
using System.Linq;
using Gma.QrCodeNet.Encoding.DataEncodation;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    public abstract class EncoderTestBase
    {
        public virtual void Test_against_reference_implementation(string inputString, int version, IEnumerable<bool> expected)
        {
            TestOneDataRow(version, inputString, expected);
        }

        public virtual void Test_against_csv_DataSet(string inputString, int version, IEnumerable<bool> expected)
        {
            TestOneDataRow(version, inputString, expected);
        }

        private void TestOneDataRow(int version, string inputString, IEnumerable<bool> expected)
        {
            EncoderBase target = CreateEncoder(version);
            IEnumerable<bool> actualResult = target.Encode(inputString);

            CollectionAssert.AreEquivalent(expected.ToList(), actualResult.ToList());
        }

        protected abstract EncoderBase CreateEncoder(int version);
    }
}