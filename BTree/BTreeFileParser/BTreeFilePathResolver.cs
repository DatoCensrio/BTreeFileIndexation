using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using BTree;

namespace BTree.BTreeFileParser
{

    struct Document : IComparable
    {
        public string name;
        public string[] text;

        public int CompareTo(object obj)
        {
            return name.CompareTo(obj as string);
        }
    }
    static class BTreeFilePathResolver
    {
       /* public static void ResolvePath(string path)
        {
            BTree<Document> bTree = new BTree<Document>(10);
            var files = Directory.GetFiles(path);
            var docs = files.Select(f => new Document() { name = f, text = File.ReadAllLines(f) });
            foreach (var item in docs)
            {
                bTree.Insert(item);
            }
            int i = 0;
            string filepath = @"E:\TestFolder";
            foreach (var item in bTree.DeployLevels())
            {
                var dir = Directory.CreateDirectory(path + "\\Dirrectory" + i);
                path = dir.FullName; i++;
                foreach (var item1 in item)
                {
                    File.Create(path + item1.name.Substring(filepath.Length));
                    //File.WriteAllLines(item1.name.Except(filepath.ToCharArray()) + path , item1.text);
                }
            }
        }*/
    }
}
