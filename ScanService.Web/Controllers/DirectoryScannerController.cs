using Microsoft.AspNetCore.Mvc;
using ScanService.Core.Scanners.Services;
using ScanService.Web.Controllers.Dto;
using ScanService.Web.Controllers.Mappers;

namespace ScanService.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DirectoryScannerController : ControllerBase
    {
        private readonly IDirectoryScanner _service;

        public DirectoryScannerController(IDirectoryScanner service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ScanResultDto GetTask(int id)
        {
            var taskResult = _service.GetScanResult(id);
            return taskResult.ToDto();
        }

        [HttpPost]
        [Route("")]
        public ScanTaskDto CreateScanTask(ScanTaskCreateDto newScanInfo)
        {
            var taskId = _service.CreateScanTask(newScanInfo.DirectoryPath);

            return new ScanTaskDto
            {
                TaskId = taskId,
                DirectoryPath = newScanInfo.DirectoryPath
            };
        }
    }
}