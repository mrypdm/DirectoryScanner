using System;

namespace ScanService.Core.Scanners
{
    public class ScanResult
    {
        public static readonly ScanResult NotFinishedScan = new() {IsFinished = false};

        public bool IsFinished { get; init; }

        public string Directory { get; init; }
        public int ProcessedFiles { get; set; }

        public int JsDetectionsCount { get; set; }
        public int RemoveFilesDetectionsCount { get; set; }
        public int RunDll32DetectionsCount { get; set; }

        public int ErrorsCount { get; set; }
        
        public TimeSpan ExecutionTime { get; set; }
    }
}