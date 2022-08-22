using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScanService.Core.Exceptions;
using ScanService.Core.Scanners;
using ScanService.Core.Scanners.Services;

namespace ScanService.Core.Tasks
{
    public class ScanTaskProvider : IScanTaskProvider
    {
        private static readonly List<Task<ScanResult>> Tasks = new ();

        private readonly IDirectoryScanner _directoryScanner;

        public ScanTaskProvider(IDirectoryScanner directoryScanner)
        {
            _directoryScanner = directoryScanner;
        }

        public int CreateScanDirectoryTask(string directoryName)
        {
            var newTask = _directoryScanner.AnalyzeDirectory(directoryName);
            
            Tasks.Add(newTask);

            return newTask.Id;
        }

        public ScanResult GetScanTaskResult(int taskId)
        {
            var currentTask = Tasks.FirstOrDefault(t => t.Id == taskId);

            if (currentTask == null)
            {
                throw new ValidationException($"Task with id='{taskId}' doesn't exist");
            }

            return currentTask.IsCompleted ? currentTask.Result : ScanResult.NotFinishedScan;
        }
    }
}