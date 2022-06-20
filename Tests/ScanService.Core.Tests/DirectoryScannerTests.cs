using System.IO;
using System.Threading.Tasks;
using ScanService.Core.Exceptions;
using ScanService.Core.Scanners.Services;
using Xunit;

namespace ScanService.Core.Tests
{
    public class DirectoryScannerTests
    {
        private readonly IDirectoryScanner _directoryScanner;

        public DirectoryScannerTests()
        {
            _directoryScanner = new DirectoryScanner();
        }
        
        
        [Fact]
        public async Task MainTest_Success()
        {
            // ARRANGE
            
            string directoryPath = Path.GetFullPath(@"../../../") + "misc/TestFolder";
            int expectedFilesCount = 15;
            int expectedErrorsCount = 0;
            int expectedJsCount = 5;
            int expectedRemoveFilesCount = 4;
            int expectedRunDllCount = 3;
            
            // ACT
            
            var task = _directoryScanner.AnalyzeDirectory(directoryPath);
            var scanResult = await task;

            // ASSERT

            Assert.Equal(expectedFilesCount, scanResult.ProcessedFiles);
            Assert.Equal(expectedErrorsCount, scanResult.ErrorsCount);
            Assert.Equal(expectedJsCount, scanResult.JsDetectionsCount);
            Assert.Equal(expectedRemoveFilesCount, scanResult.RemoveFilesDetectionsCount);
            Assert.Equal(expectedRunDllCount, scanResult.RunDll32DetectionsCount);
        }

        [Fact]
        public void ScanNotExistingDirectory_ShouldThrow()
        {
            // ARRNGE

            string notExistingDirectoryPath = "./long_directory_name";
            
            // ACT, ASERT

            Assert.ThrowsAsync<ValidationException>(() => _directoryScanner.AnalyzeDirectory(notExistingDirectoryPath));
        }
        
        [Fact]
        public void ScanNotDirectory_ShouldThrow()
        {
            // ARRNGE

            string notDirectoryPath = "./file.txt";
            
            // ACT, ASERT

            Assert.ThrowsAsync<ValidationException>(() => _directoryScanner.AnalyzeDirectory(notDirectoryPath));
        }
    }
}
