using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Linq;

namespace BTree.Tests
{
    class Tests
    {
        [TestCase (new[] {1 ,8, 5, 3, 7, 6, 6, 1, 7, 67, 32, 15 , 11, 87})]
        [TestCase(new[] {56, 75, 54 , 65, 24 ,76 ,  23, 76, 231, 746, 24 })]
        [TestCase(new[] { 1, 67 ,3657, 34 ,23, 76, 34 ,672, 45})]
        [TestCase(new[] { 0 , 1, 2 ,3, 4 ,5, 6, 7, 8 })]
        public void TestIntegers()
        {
            TestCaseData data = new TestCaseData();
            var arrays = data.Arguments.Select(a => (IEnumerable<int>)a).ToArray();
            foreach (var item in arrays)
            {
                for (int i = 0; i < 5; i++)
                {
                    BTree<int, int> bTree = new BTree<int, int>(i);
                    Assert.AreEqual(bTree.Count(), item.Count());
                    var e1 = item.OrderBy(k => k).GetEnumerator(); e1.MoveNext(); var e2 = bTree.ToArray();
                    for (int g = 0; g < e2.Length; g++)
                    {
                        Assert.AreEqual(e1.Current , e2[i]);
                        e1.MoveNext();
                    }
                }
            }
        }
    }
}
