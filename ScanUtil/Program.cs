using System;
using System.Text;
using System.Threading.Tasks;
using ScanUtil.Dto;

namespace ScanUtil
{
    internal static class Program
    {
        private static void ShowHelp()
        {
            Console.WriteLine("Possible Commands:");
            Console.WriteLine("- scan <DirectoryPath>");
            Console.WriteLine("- status <TaskId>");
        }

        private static string ComposeReport(ScanResultDto result)
        {
            if (!result.IsFinished)
            {
                return "Scan task in progress, please wait";
            }

            StringBuilder builder = new();

            builder.AppendLine("====== Scan result ======");
            builder.AppendLine($"Directory: {result.Directory}");
            builder.AppendLine($"Processed files: {result.ProcessedFiles}");
            builder.AppendLine($"JS detects: {result.JsDetectionsCount}");
            builder.AppendLine($"rm -rf detects: {result.RemoveFilesDetectionsCount}");
            builder.AppendLine($"RunDll32 detects: {result.RunDll32DetectionsCount}");
            builder.AppendLine($"Errors: {result.ErrorsCount}");
            builder.AppendLine($"Execution time: {result.ExecutionTime:hh\\:mm\\:ss}");
            builder.Append("=========================");

            return builder.ToString();
        }

        private static async Task RunCmd(string cmd, string arg)
        {
            var client = new ScanServiceClient();

            switch (cmd)
            {
                case "scan":
                {
                    var result = await client.StartScan(arg);
                    Console.WriteLine($"Scan task was created with ID: {result.TaskId}");
                    break;
                }
                case "status":
                {
                    var result = await client.GetStatus(int.Parse(arg));
                    Console.WriteLine(ComposeReport(result));
                    break;
                }
                default:
                {
                    ShowHelp();
                    break;
                }
            }
        }

        public static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                ShowHelp();
            }
            else
            {
                try
                {
                    await RunCmd(args[0], args[1]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}