using System;
using System.IO;
using System.Threading.Tasks;
using ScanService.Core.Exceptions;
using ScanService.Core.Scanners;
using ScanService.Core.Scanners.Services;
using ScanService.Core.Tasks;
using Xunit;

namespace ScanService.Core.Tests
{
    public class TaskProviderMock : ITaskProvider
    {
        public Task CurrentTask;
        
        public Task<TResult> CreateTask<TResult>(Func<Task<TResult>> func)
        {
            var newTask = Task.Run(func);
            CurrentTask = newTask;
            return newTask;
        }
    }
    
    public class DirectoryScannerTests
    {
        private readonly TaskProviderMock _taskProviderMock;
        private readonly IDirectoryScanner _directoryScanner;

        public DirectoryScannerTests()
        {
            _taskProviderMock = new TaskProviderMock();
            _directoryScanner = new DirectoryScanner(_taskProviderMock);
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

            _directoryScanner.CreateScanTask(directoryPath);
            
            var task = _taskProviderMock.CurrentTask as Task<ScanResult>;
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

            Assert.Throws<ValidationException>(() => _directoryScanner.CreateScanTask(notExistingDirectoryPath));
        }
        
        [Fact]
        public void ScanNotDirectory_ShouldThrow()
        {
            // ARRNGE

            string notDirectoryPath = "./file.txt";
            
            // ACT, ASERT

            Assert.Throws<ValidationException>(() => _directoryScanner.CreateScanTask(notDirectoryPath));
        }
    }
}
