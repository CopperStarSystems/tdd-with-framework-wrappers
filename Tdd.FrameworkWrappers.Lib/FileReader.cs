using System;
using Tdd.FrameworkWrappers.Lib.FrameworkWrappers;

namespace Tdd.FrameworkWrappers.Lib
{
    public class FileReader
    {
        private readonly IFile file;
        private readonly ILogger logger;

        public FileReader(IFile file, ILogger logger)
        {
            this.file = file;
            this.logger = logger;
        }

        public string ReadText(string filePath)
        {
            try
            {
                return file.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                string message = $"Error reading file {filePath}";
                logger.Log(LogLevelEnum.Error, message);
                throw;
            }
        }
    }
}