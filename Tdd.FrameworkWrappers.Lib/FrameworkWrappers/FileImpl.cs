using System.IO;

namespace Tdd.FrameworkWrappers.Lib.FrameworkWrappers
{
    public class FileImpl : IFile
    {
        public string ReadAllText(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}