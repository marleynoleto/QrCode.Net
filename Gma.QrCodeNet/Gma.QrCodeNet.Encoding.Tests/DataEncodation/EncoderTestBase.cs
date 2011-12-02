using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gma.QrCodeNet.Encoding.DataEncodation;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    public abstract class EncoderTestBase
    {
        public virtual void Test_against_reference_implementation(string inputString, IEnumerable<bool> expected)
        {
            TestOneDataRow(inputString, expected);
        }

        public virtual void Test_against_csv_DataSet(string inputString, IEnumerable<bool> expected)
        {
            TestOneDataRow(inputString, expected);
        }

        private void TestOneDataRow(string inputString, IEnumerable<bool> expected)
        {
            EncoderBase target = CreateEncoder();
            IEnumerable actualResult = target.GetDataBits(inputString);

            CollectionAssert.AreEquivalent(expected.ToList(), actualResult);
        }

        protected abstract EncoderBase CreateEncoder();
    }
}