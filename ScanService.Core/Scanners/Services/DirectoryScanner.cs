using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ScanService.Core.Exceptions;
using ScanService.Core.Tasks;

namespace ScanService.Core.Scanners.Services
{
    public class DirectoryScanner : IDirectoryScanner
    {
        private static readonly List<Task<ScanResult>> Tasks = new();

        private const string JavaScriptSus = "<script>evil_script()</script>";
        private const string RemoveFilesSus = @"rm -rf %userprofile%\Documents";
        private const string RunDll32Sus = "Rundll32 sus.dll SusEntry";

        private readonly ITaskProvider _taskProvider;

        private CheckResults CheckLine(string line)
        {
            // "файл, содержащий строку"
            // Т.к. не сказано "содержащий текст" и не сказано, что срока может быть с пробелами в начале/конце, то сравниваем в лоб
            return line switch
            {
                JavaScriptSus => CheckResults.JavaScript,
                RemoveFilesSus => CheckResults.RemoveFiles,
                RunDll32Sus => CheckResults.RunDll32,
                _ => CheckResults.Ok
            };
        }

        private async Task<CheckResults> CheckFileForSuspicion(FileInfo file)
        {
            StreamReader reader = null;
            try
            {
                reader = file.OpenText();
                while (!reader.EndOfStream)
                {
                    var currentLine = await reader.ReadLineAsync();

                    var lineCheckResult = CheckLine(currentLine);
                    
                    // Если нашли паттерн не в .js файле, то это норм
                    var jsSusInNotJsFile = lineCheckResult == CheckResults.JavaScript && file.Extension != ".js";
                    if (lineCheckResult != CheckResults.Ok && !jsSusInNotJsFile)
                    {
                        return lineCheckResult;
                    }
                }
            }
            catch (Exception)
            {
                return CheckResults.Error;
            }
            finally
            {
                reader?.Dispose();
            }

            return CheckResults.Ok;
        }

        private async Task<ScanResult> AnalyzeDirectory(string directoryPath)
        {
            var stopWatch = Stopwatch.StartNew();

            var directoryInfo = new DirectoryInfo(directoryPath);

            var taskResult = new ScanResult
            {
                IsFinished = true,
                Directory = directoryPath
            };
            
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                var fileCheckResult = await CheckFileForSuspicion(fileInfo);

                switch (fileCheckResult)
                {
                    case CheckResults.JavaScript:
                        taskResult.JsDetectionsCount++;
                        break;
                    case CheckResults.RemoveFiles:
                        taskResult.RemoveFilesDetectionsCount++;
                        break;
                    case CheckResults.RunDll32:
                        taskResult.RunDll32DetectionsCount++;
                        break;
                    case CheckResults.Error:
                        taskResult.ErrorsCount++;
                        break;
                }

                taskResult.ProcessedFiles++;
            }

            stopWatch.Stop();

            taskResult.ExecutionTime = stopWatch.Elapsed;

            return taskResult;
        }

        public DirectoryScanner(ITaskProvider taskProvider)
        {
            _taskProvider = taskProvider;
        }
        
        public ScanResult GetScanResult(int taskId)
        {
            var currentTask = Tasks.FirstOrDefault(t => t.Id == taskId);

            if (currentTask == null)
            {
                throw new ValidationException($"Task with id='{taskId}' doesn't exist");
            }

            return currentTask.IsCompleted ? currentTask.Result : ScanResult.NotFinishedScan;
        }

        public int CreateScanTask(string directoryPath)
        {
            var fullDirectoryPath = Environment.ExpandEnvironmentVariables(directoryPath);
            
            if (!Directory.Exists(fullDirectoryPath))
            {
                throw new ValidationException($"Path '{fullDirectoryPath}' doesn't exists or it is not a directory");
            }

            var newTask = _taskProvider.CreateTask(() => AnalyzeDirectory(fullDirectoryPath));
            Tasks.Add(newTask);

            return newTask.Id;
        }
    }
}