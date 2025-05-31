using System;

namespace OODP.Interfaces
{
    public interface IFileService
    {
        void WriteAllText(string path, string contents);
        string ReadAllText(string path);
        bool Exists(string path);
    }
}

