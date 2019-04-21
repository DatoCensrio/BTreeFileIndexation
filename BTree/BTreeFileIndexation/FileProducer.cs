using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace BTree.BTreeFileIndexation
{
    class FileProducer
    {
        public string Path { get; set; }

        public FileProducer(string initialPath)
        {
            Path = initialPath;
        }

        public IEnumerable<string> GetAllFilesInSubDirectories()
        {
            var directories = DeployDirectories(Path);
            var files = directories.SelectMany(d =>
            {
                string[] f;
                try
                {
                    f = Directory.GetFiles(d);
                }
                catch (Exception)
                {
                    f = new string[] { };
                }
                return f;
            });
            return files;
        }

        private IEnumerable<string> DeployDirectories(string iniialPath)
        {
            bool valid = true;
            try
            {
                Directory.GetDirectories(iniialPath);
            }
            catch (Exception)
            {
                valid = false;
            }
            if (valid)
            {
                foreach (var dir in Directory.GetDirectories(iniialPath))
                {
                    yield return dir;
                    foreach (var subDir in DeployDirectories(dir))
                    {
                        yield return dir;
                    }
                }
            }
        }
    }
}
