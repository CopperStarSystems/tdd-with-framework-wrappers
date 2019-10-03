using System.IO;
using System.Net;

namespace Tdd.FrameworkWrappers.Lib
{
    public class FileReader
    {
        public string ReadText(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}