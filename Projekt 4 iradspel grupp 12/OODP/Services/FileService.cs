using System.IO;
using OODP.Interfaces;

namespace OODP.Services
{
    public class FileService : IFileService
    {
        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}
