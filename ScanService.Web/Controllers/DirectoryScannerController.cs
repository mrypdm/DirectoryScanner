using Microsoft.AspNetCore.Mvc;
using ScanService.Core.Tasks;
using ScanService.Web.Controllers.Dto;
using ScanService.Web.Controllers.Mappers;

namespace ScanService.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DirectoryScannerController : ControllerBase
    {
        private readonly IScanTaskProvider _service;

        public DirectoryScannerController(IScanTaskProvider service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ScanResultDto GetTask(int id)
        {
            var taskResult = _service.GetScanTaskResult(id);
            return taskResult.ToDto();
        }

        [HttpPost]
        [Route("")]
        public ScanTaskDto CreateScanTask(ScanTaskCreateDto newScanInfo)
        {
            var taskId = _service.CreateScanDirectoryTask(newScanInfo.DirectoryPath);

            return new ScanTaskDto
            {
                TaskId = taskId,
                DirectoryPath = newScanInfo.DirectoryPath
            };
        }
    }
}