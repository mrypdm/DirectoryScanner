namespace ScanService.Web.Controllers.Dto
{
    public class ScanTaskCreateDto
    {
        public string DirectoryPath { get; set; }
    }

    public class ScanTaskDto
    {
        public string DirectoryPath { get; set; }
        public int TaskId { get; set; }
    }
}