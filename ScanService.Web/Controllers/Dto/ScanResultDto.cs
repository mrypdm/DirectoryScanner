using System;

namespace ScanService.Web.Controllers.Dto
{
    public class ScanResultDto
    {
        public bool IsFinished { get; set; }
    }
    
    public class ScanResultDetailDto : ScanResultDto
    {   
        public string Directory { get; set; }
        public int ProcessedFiles { get; set; }
        
        public TimeSpan ExecutionTime { get; set; }
        
        public int JsDetectionsCount { get; set; }
        public int RemoveFilesDetectionsCount { get; set; }
        public int RunDll32DetectionsCount { get; set; }
        
        public int ErrorsCount { get; set; }
    }
}