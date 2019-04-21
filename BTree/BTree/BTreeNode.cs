using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BTree
{
    public class Entity<Tkey, TVal> where Tkey : IComparable
    {
        public Tkey Key { get; set; }
        public TVal Value { get; set; }

        public Entity(Tkey key, TVal val)
        {
            Key = key;
            Value = val;
        }
    }
    public class BTreeNode<TKey, TVal> where TKey : IComparable
    {
        public int Degree { get; set; }
        public List<Entity<TKey, TVal>> Items { get; set; }
        public List<BTreeNode<TKey, TVal>> Nodes { get; set; }
        public BTreeNode<TKey, TVal> Parent { get; set; }
        public BTreeNode(int degree)
        {
            Degree = degree;
            Items = new List<Entity<TKey, TVal>>();
            Nodes = new List<BTreeNode<TKey, TVal>>();
        }
        public void InsertELement(TKey key, TVal val)
        {
            if (Items.Count == 0)
            {
                Nodes.Add(new BTreeNode<TKey, TVal>(Degree) { Parent = this });
            }
            int nodeIndex = FindTransitionIndex(key);
            if (nodeIndex == 0)
            {
                if (Nodes[0].Items.Count != 0 && Nodes[0].Items[0].Key.CompareTo(key) <= 0)
                {
                    Items.Insert(0, new Entity<TKey, TVal>(key, val));
                    Nodes.Insert(1, new BTreeNode<TKey, TVal>(Degree));
                }
                else
                {
                    Items.Insert(0, new Entity<TKey, TVal>(key, val));
                    Nodes.Insert(0, new BTreeNode<TKey, TVal>(Degree) { Parent = this });
                }
            }
            else
            {
                if (Nodes[nodeIndex].Items.Count != 0 && Nodes[nodeIndex].Items[0].Key.CompareTo(key) <= 0)
                {
                    Nodes.Insert(nodeIndex + 1, new BTreeNode<TKey, TVal>(Degree) { Parent = this });
                    Items.Insert(nodeIndex, new Entity<TKey, TVal>(key, val));
                }
                else
                {
                    Nodes.Insert(nodeIndex , new BTreeNode<TKey, TVal>(Degree) { Parent = this });
                    Items.Insert(nodeIndex, new Entity<TKey, TVal>(key, val));
                }
            }
        }

        public bool Find(TKey key, out TVal val)
        {
            if (Items.Count == 0)
            {
                val = default(TVal);
                return false;
            }
            var item = Items.Where(i => i.Key.CompareTo(key) == 0);
            if (item.Count() == 0)
            {
                return Nodes[FindTransitionIndex(key)].Find(key, out val);
            }
            else
            {
                val = item.First().Value;
                return true;
            }
        }
        public bool Contains(TKey key)
        {
            if (Items.Count == 0)
            {
                return false;
            }
            if (Items.Any(i => i.Key.CompareTo(key) == 0))
            {
                return true;
            }
            else
            {
                int index = FindTransitionIndex(key);
                return Nodes[index].Contains(key);
            }
        }

        public int FindTransitionIndex(TKey key)
        {
            if (Items.Count == 0 || key.CompareTo(Items[0].Key) <= 0)
            {
                return 0;
            }
            if (Items.Count == 1)
            {
                return Items[0].Key.CompareTo(key) > 0 ? 0 : 1;
            }
            if (key.CompareTo(Items.Last().Key) > 0)
            {
                return Nodes.Count - 1;
            }
            else
            {
                for (int i = 1; i < Items.Count; i++)
                {
                    if (Items[i].Key.CompareTo(key) >= 0 && Items[i - 1].Key.CompareTo(key) < 0)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }
        public IEnumerable<TVal> DeployTreeNode()
        {
            if (Items == null || Items.Count == 0)
            {
                yield break;
            }
            else
            {
                foreach (var item in Nodes[0].DeployTreeNode())
                {
                    yield return item;
                }
                for (int i = 1; i < Nodes.Count; i++)
                {
                    yield return Items[i - 1].Value;
                    foreach (var item in Nodes[i].DeployTreeNode())
                    {
                        yield return item;
                    }
                }
            }
        }

        public bool IsRoot
        {
            get => Parent == null;
        }
        public bool ReachedMaxChildCount
        {
            get => !IsRoot ? Items.Count >= (2 * Degree) - 1 : Items.Count >= Degree - 1;
        }

        public bool IsLeaf
        {
            get => Nodes.Count == 0 || Nodes.Select(n => n.Items.Count).Sum() == 0;
        }
    }
}
