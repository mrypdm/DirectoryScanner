namespace ScanService.Core.Scanners.Services
{
    public interface IDirectoryScanner
    {
        ScanResult GetScanResult(int taskId);

        int CreateScanTask(string directoryPath);
    }
}