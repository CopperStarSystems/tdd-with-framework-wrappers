using System.IO;
using NUnit.Framework;

namespace Tdd.FrameworkWrappers.Lib.Tests
{
    [TestFixture]
    public class FileReaderTests
    {
        [SetUp]
        public void SetUp()
        {
            systemUnderTest = new FileReader();
        }

        private FileReader systemUnderTest;

        private void CleanupTestFile(string filePath)
        {
            File.Delete(filePath);
        }

        private void CreateTestFile(string filePath, string fileContent)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(filePath, fileContent);
        }

        [Test]
        public void ReadText_Always_PerformsExpectedWork()
        {
            // This test can't actually test anything because the SystemUnderTest uses the static File
            // class - in other words, we can't mock File to verify that ReadAllText is called.
        }

        [Test]
        public void ReadText_WhenFileExists_ReturnsFileContents()
        {
            const string filePath = @"c:\temp\test.txt";
            const string fileContent = "The quick brown fox jumps over the lazy dog.";
            CreateTestFile(filePath, fileContent);
            var result = systemUnderTest.ReadText(filePath);
            Assert.That(result, Is.EqualTo(fileContent));
            CleanupTestFile(filePath); 
        }
    }
}