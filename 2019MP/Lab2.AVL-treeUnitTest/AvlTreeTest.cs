using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab2.AVL_tree;

namespace Lab2.AVL_treeUnitTest
{
    [TestClass]
    public class AvlTreeTest
    {
        [TestMethod]
        public void AvlTreeConstructorWithoutException()
        {
            var tree = new AvlTree<int, int>();
        }

        [TestMethod]
        public void CountOfEmptyTree()
        {
            var tree = new AvlTree<int, int>();
            Assert.AreEqual(0, tree.Count);
        }

        [TestMethod]
        public void CountAfterAdding()
        {
            var tree = new AvlTree<int, int>();
            tree.Add(0, 0);
            tree.Add(1, 0);
            Assert.AreEqual(2, tree.Count);
        }

        [TestMethod]
        public void AddOneElement()
        {
            var tree = new AvlTree<int, int>();
            tree.Add(0, 0);
            Assert.AreEqual("0 0\n", tree.Print());
        }

        [TestMethod]
        public void AddTwoElementSecondIsLessFirst()
        {
            var tree = new AvlTree<int, int>();
            tree.Add(0, 0);
            tree.Add(-1, 0);
            Assert.AreEqual("-1 0\n0 0\n", tree.Print());
        }

        [TestMethod]
        public void AddTwoElementSecondIsGreaterFirst()
        {
            var tree = new AvlTree<int, int>();
            tree.Add(0, 0);
            tree.Add(1, 0);
            Assert.AreEqual("0 0\n1 0\n", tree.Print());
        }

        [TestMethod]
        public void TestIndexer()
        {
            var tree = new AvlTree<int, int>();
            for (var i = 0; i < 100; i++)
                tree.Add(i, i);

            for (var i = 0; i < 100; i++)
                Assert.AreEqual(i, tree[i]);
        }

        [TestMethod]
        public void TestContainsAfterRemove()
        {
            var tree = new AvlTree<int, int>();
            for (var i = 0; i < 100; i++)
                tree.Add(i, i);

            tree.Remove(25);

            Assert.IsFalse(tree.ContainsKey(25));
        }
    }
}
