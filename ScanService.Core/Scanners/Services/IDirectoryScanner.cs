using System.Threading.Tasks;

namespace ScanService.Core.Scanners.Services
{
    public interface IDirectoryScanner
    {
        Task<ScanResult> AnalyzeDirectory(string directoryPath);
    }
}