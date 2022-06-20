using ScanService.Core.Scanners;
using ScanService.Web.Controllers.Dto;

namespace ScanService.Web.Controllers.Mappers
{
    public static class ScanResultMappers
    {
        public static ScanResultDto ToDto(this ScanResult result)
        {
            if (!result.IsFinished)
            {
                return new ScanResultDto {IsFinished = false};
            }
            
            return new ScanResultDetailDto
            {
                IsFinished = result.IsFinished,
                
                Directory = result.Directory,
                ProcessedFiles = result.ProcessedFiles,
                ExecutionTime = result.ExecutionTime,
                
                JsDetectionsCount = result.JsDetectionsCount,
                RemoveFilesDetectionsCount = result.RemoveFilesDetectionsCount,
                RunDll32DetectionsCount = result.RunDll32DetectionsCount,
                ErrorsCount = result.ErrorsCount
            };
        }
    }
}