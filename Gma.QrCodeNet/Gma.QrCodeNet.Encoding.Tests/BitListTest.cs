using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gma.QrCodeNet.Encoding.Tests
{
    [TestClass]
    public class BitListTest
    {
        [TestMethod]
        public void Count_is_0_after_costruction()
        {
            BitList target = new BitList();
            Assert.AreEqual(0, target.Count);
        }

        [TestMethod]
        public void Insert_1_count_is_1()
        {
            BitList target = new BitList();
            target.Add(true);
            Assert.AreEqual(1, target.Count);
        }

        [TestMethod]
        public void Insert_0_count_is_1()
        {
            BitList target = new BitList();
            target.Add(true);
            Assert.AreEqual(1, target.Count);
        }

    }
}
