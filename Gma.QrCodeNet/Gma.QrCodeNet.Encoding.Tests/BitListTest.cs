using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests
{
    [TestFixture]
    public class BitListTest
    {
        [Test]
        public void Count_is_0_after_costruction()
        {
            BitList target = new BitList();
            Assert.AreEqual(0, target.Count);
        }

        [Test]
        public void Insert_1_count_is_1()
        {
            BitList target = new BitList();
            target.Add(true);
            Assert.AreEqual(1, target.Count);
        }

        [Test]
        public void Insert_0_count_is_1()
        {
            BitList target = new BitList();
            target.Add(true);
            Assert.AreEqual(1, target.Count);
        }

    }
}
