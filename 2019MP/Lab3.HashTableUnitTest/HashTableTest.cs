using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab3.HashTable;

namespace Lab3.HashTableUnitTest
{
    [TestClass]
    public class HashTableTest
    {
        [TestMethod]
        public void CountOfNewHashTableIsZero()
        {
            var ht = new HashTable<int, int>();
            Assert.AreEqual(0, ht.Count);
        }

        [TestMethod]
        public void AfterAddCountIsInc()
        {
            var ht = new HashTable<int, int>();
            for (var i = 1; i < 1000; i++)
            {
                ht.Add(i, i);
                Assert.AreEqual(i, ht.Count);
            }
        }

        [TestMethod]
        public void EnumerableIsCorrect()
        {
            var ht = new HashTable<int, int>();
            for (var i = 1; i <= 10000; i++)
                ht.Add(i, i);

            var actualCnt = 0;
            foreach (var _ in ht)
            {
                actualCnt++;
            }

            Assert.AreEqual(10000, actualCnt);
        }

        [TestMethod]
        public void TestContains()
        {
            var ht = new HashTable<int, int>();
            for (var i = 1; i <= 10000; i++)
                ht.Add(i, i);

            for (var i = 1; i <= 10000; i++)
                Assert.IsTrue(ht.Contains(i));
        }

        [TestMethod]
        public void TestRemove()
        {
            var ht = new HashTable<int, int>();
            for (var i = 1; i <= 10000; i++)
                ht.Add(i, i);

            for (var i = 1; i < 5000; i++)
                ht.Remove(i);

            for (var i = 10001; i <= 20000; i++)
                ht.Add(i, i);

            for (var i = 5000; i <= 20000; i++)
                Assert.IsTrue(ht.Contains(i));
        }

        [TestMethod]
        public void AfterRemoveCountIsDec()
        {
            var ht = new HashTable<int, int>();
            for (var i = 1; i < 1000; i++)
            {
                ht.Add(i, i);
            }
            for (var i = 999; i > 0; i--)
            {
                ht.Remove(i);
                Assert.AreEqual(i-1,ht.Count);
            }
        }
    }
}
