using System;
using BTree.BTreeFileParser;
using BTree.BTreeFileIndexation;
using BTree.Tests;

namespace BTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var indexer = new BTreeIndexation(@"F:\");
            while (true)
            {
                Console.WriteLine("Enter the file you want to reach");
                var name = Console.ReadLine();
                var file = indexer.GetFilePath(name);
                if (file == "NotFound")
                {
                    Console.WriteLine(file);
                }
                else
                {
                    Console.WriteLine(file);
                }
            }
        }
    }
}
