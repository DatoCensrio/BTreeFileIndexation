using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace BTree.BTreeFileParser
{
    static class FIleProducer
    {
        static public void ProduceToCatalog(string path , int filesCount)
        {
            for (int i = 0; i < filesCount; i++)
            {
                try
                {
                    File.Create(path + "/" + GetRandomName() + "." + GetRandomExtesion());
                }
                catch (Exception)
                {
                }
            }
        }

        static private string GetRandomName()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return Enumerable.Repeat(0, r.Next(0, 15)).Select(n => ((char)r.Next(81, 120)).ToString())
                .Where(c => char.IsLetter(c.ToCharArray()[0]))
                .Aggregate((c1, c2) => c1 + c2);
        }

        static private string GetRandomExtesion()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            return Enumerable.Repeat(0, r.Next(2, 5)).Select(n => ((char)r.Next(81, 120)).ToString()).Aggregate((c1, c2) => c1 + c2);
        }
    }
}
