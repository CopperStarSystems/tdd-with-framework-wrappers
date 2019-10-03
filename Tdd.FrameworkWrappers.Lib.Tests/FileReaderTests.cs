using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Tdd.FrameworkWrappers.Lib.FrameworkWrappers;

namespace Tdd.FrameworkWrappers.Lib.Tests
{
    [TestFixture]
    public class FileReaderTests
    {
        [SetUp]
        public void SetUp()
        {
            mockFile = new Mock<IFile>(MockBehavior.Strict);
            systemUnderTest = new FileReader(mockFile.Object);
        }

        private Mock<IFile> mockFile;
        private FileReader systemUnderTest;

        [TestCaseSource(nameof(ReadTextTestCases))]
        public void ReadText_Always_PerformsExpectedWork(string filePath, string fileContent)
        {
            mockFile.Setup(p => p.ReadAllText(filePath)).Returns(fileContent);
            systemUnderTest.ReadText(filePath);
            mockFile.VerifyAll();
        }

        [TestCaseSource(nameof(ReadTextTestCases))]
        public void ReadText_WhenFileExists_ReturnsFileContents(string filePath, string fileContent)
        {
            mockFile.Setup(p => p.ReadAllText(filePath)).Returns(fileContent);
            var result = systemUnderTest.ReadText(filePath);
            Assert.That(result, Is.EqualTo(fileContent));
        }

        public static IEnumerable<TestCaseData> ReadTextTestCases
        {
            get
            {
                yield return new TestCaseData(@"c:\temp\test.txt", "Some file content.");
                yield return new TestCaseData(@"c:\path\to\file.txt", "The quick brown fox jumps over the lazy dog.");
                yield return new TestCaseData(@"c:\other\path\somefile.txt", string.Empty);
            }
        }
    }
}