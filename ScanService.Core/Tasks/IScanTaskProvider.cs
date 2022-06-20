using ScanService.Core.Scanners;

namespace ScanService.Core.Tasks
{
    public interface IScanTaskProvider
    {
        int CreateScanDirectoryTask(string directoryName);

        ScanResult GetScanTaskResult(int taskId);
    }
}