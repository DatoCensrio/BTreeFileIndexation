using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BTree
{
    class BTree<TKey, TVal> : IEnumerable<TVal> where TKey : IComparable
    {
        public int Count { get; private set; }
        public BTreeNode<TKey, TVal> Head { get; private set; }
        public int Degree { get; private set; }
        public BTree(int degree)
        {
            Degree = degree;
        }
        public TVal this[int index]
        {
            get { return this.Skip(index).First(); }
        }
        public bool Find(TKey key, out TVal val)
        {
            return Head.Find(key, out val);
        }
        public void Insert(TKey key, TVal val)
        {
            Count++;
            if (Head == null)
            {
                Head = new BTreeNode<TKey, TVal>(Degree + 1);
                Head.InsertELement(key, val);
            }
            else if (!Head.ReachedMaxChildCount && Head.IsLeaf)
            {
                Head.InsertELement(key, val);
            }
            else
            {
                Insert(key, val, Head);
            }
        }

        public void Delete(TKey key)
        {

        }
        private void Insert(TKey key, TVal val, BTreeNode<TKey, TVal> node)
        {
            int index = node.FindTransitionIndex(key);
            if (node.Nodes[index].IsLeaf)
            {
                if (!node.Nodes[index].ReachedMaxChildCount)
                {
                    node.Nodes[index].InsertELement(key, val);
                }
                else
                {
                    node.Nodes[index].InsertELement(key , val);
                    SliceChild(node, index);   
                }
            }
            else
            {
                Insert(key, val, node.Nodes[index]);
            }
        }

        private void SliceChild(BTreeNode<TKey, TVal> node, int childInd)
        {
            var child = node.Nodes[childInd];
            int middle = child.Items.Count / 2;
            BTreeNode<TKey, TVal> newNodeLeft = new BTreeNode<TKey, TVal>(Degree); newNodeLeft.Parent = node;
            BTreeNode<TKey, TVal> newNodeRight = new BTreeNode<TKey, TVal>(Degree); newNodeRight.Parent = node;
            for (int i = 0; i < middle; i++)
            {
                newNodeLeft.Items.Add(child.Items[i]); newNodeLeft.Nodes.Add(child.Nodes[i]); newNodeLeft.Nodes.Last().Parent = newNodeLeft;
            }
            newNodeLeft.Nodes.Add(child.Nodes[middle]); newNodeLeft.Nodes.Last().Parent = newNodeLeft;
            for (int i = middle + 1; i < child.Items.Count; i++)
            {
                newNodeRight.Items.Add(child.Items[i]); newNodeRight.Nodes.Add(child.Nodes[i]); newNodeRight.Nodes.Last().Parent = newNodeRight;
            }
            newNodeRight.Nodes.Add(child.Nodes.Last()); newNodeRight.Nodes.Last().Parent = newNodeRight;
            if (childInd == 0)
            {
                node.Items.Insert(0, child.Items[middle]);
                node.Nodes[0] = newNodeRight;
                node.Nodes.Insert(0, newNodeLeft);
            }
            else
            {
                node.Items.Insert(childInd, child.Items[middle]);
                node.Nodes[childInd] = newNodeLeft;
                node.Nodes.Insert(childInd + 1, newNodeRight);
            }
            if (node.ReachedMaxChildCount)
            {
                if (node.IsRoot)
                {
                    SliceRoot();
                }
                else
                {
                    SliceChild(node.Parent, node.Parent.Nodes.IndexOf(node));
                }
            }
        }

        private void SliceRoot()
        {
            int middle = Head.Items.Count / 2;
            BTreeNode<TKey, TVal> newNodeLeft = new BTreeNode<TKey, TVal>(Degree) { Parent = Head};
            BTreeNode<TKey, TVal> newNodeRight = new BTreeNode<TKey, TVal>(Degree) { Parent = Head};
            for (int i = 0; i < middle; i++)
            {
                newNodeLeft.Items.Add(Head.Items[i]); newNodeLeft.Nodes.Add(Head.Nodes[i]); newNodeLeft.Nodes.Last().Parent = newNodeLeft;
            }
            newNodeLeft.Nodes.Add(Head.Nodes[middle]); newNodeLeft.Nodes.Last().Parent = newNodeLeft;
            for (int i = middle + 1; i < Head.Items.Count; i++)
            {
                newNodeRight.Items.Add(Head.Items[i]); newNodeRight.Nodes.Add(Head.Nodes[i]); newNodeRight.Nodes.Last().Parent = newNodeRight;
            }
            newNodeRight.Nodes.Add(Head.Nodes.Last()); newNodeRight.Nodes.Last().Parent = newNodeRight;
            var middleItem = Head.Items[middle];
            Head.Items = new List<Entity<TKey, TVal>>();
            Head.Nodes = new List<BTreeNode<TKey, TVal>>();
            Head.Items.Insert(0, middleItem);
            Head.Nodes.Add(newNodeLeft);
            Head.Nodes.Add(newNodeRight);
        }

        public IEnumerable<IEnumerable<TVal>> DeployLevels()
        {
            yield return Head.Items.Select(i => i.Value);
            var level = Head.Nodes;
            while (true)
            {
                var ret = level.SelectMany(n => n.Items).Select(e => e.Value);
                yield return ret;
                level = level.SelectMany(n => n.Nodes).ToList();
                if (level.Count == 0)
                {
                    break;
                }
            }
        }

        IEnumerator<TVal> IEnumerable<TVal>.GetEnumerator()
        {
            if (Head == null)
            {
                yield break;
            }
            foreach (var item in Head.DeployTreeNode())
            {
                yield return item;
            }
        }

        public IEnumerator GetEnumerator()
        {
            if (Head == null)
            {
                yield break;
            }
            foreach (var item in Head.DeployTreeNode())
            {
                yield return item;
            }
        }
    }
}
