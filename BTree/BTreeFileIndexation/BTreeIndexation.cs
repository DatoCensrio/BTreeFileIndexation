using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BTree.BTreeFileIndexation
{
    class BTreeIndexation
    {
        BTree<string , string> BTree { get; set; }
        public BTreeIndexation(string initialPath)
        {
            BTree = new BTree<string, string>(10);
            foreach (var item 
                in new FileProducer(initialPath).GetAllFilesInSubDirectories().Distinct())
            {
                BTree.Insert(item.Split('\\').Last().Split('.')[0] , item);
            }
        }

        public string GetFilePath(string fileName)
        {
            string file;
            bool find = BTree.Find(fileName, out file);
            if (find)
            {
                return file;
            }
            else
            {
                return "NotFound";
            }
        }
    }
}
