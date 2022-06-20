using System;
using System.Text.Json.Serialization;

namespace ScanUtil.Dto
{
    public class ScanResultDto
    {
        public bool IsFinished { get; set; }
        public string Directory { get; set; }
        public int ProcessedFiles { get; set; }

        [JsonConverter(typeof(JsonTimeSpanConverter.JsonTimeSpanConverter))]
        public TimeSpan ExecutionTime { get; set; }

        public int JsDetectionsCount { get; set; }
        public int RemoveFilesDetectionsCount { get; set; }
        public int RunDll32DetectionsCount { get; set; }

        public int ErrorsCount { get; set; }
    }
}