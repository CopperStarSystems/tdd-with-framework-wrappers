using System.IO;
using System.Net;
using Tdd.FrameworkWrappers.Lib.FrameworkWrappers;

namespace Tdd.FrameworkWrappers.Lib
{
    public class FileReader
    {
        private readonly IFile file;

        public FileReader(IFile file)
        {
            this.file = file;
        }
        public string ReadText(string filePath)
        {
            return file.ReadAllText(filePath);
        }
    }
}