using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileShare.Services.Interfaces;
using FileShare.Contracts;

namespace FileShare.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _service;
        private readonly ILogger<FileController> _logger;

        public FileController(
            ILogger<FileController> logger, IFileService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<FileDto>> Get(Guid roomId)
        {
            return await _service.GetFiles(roomId);
        }

        [HttpGet("{id}")]
        public async Task<FileDto> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception("No ID provided");          
            return await _service.GetFileInfo(id);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception("No file ID provided");          
            var fileInfo = await _service.GetFileInfo(id);
            var results = await _service.GetFileDownload(fileInfo.RoomId, fileInfo.Name);
            return new FileContentResult(results.ToArray(), "application/octet-stream")
            {
                FileDownloadName = fileInfo.Name
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateFileDto dto)
        {
            if (dto == null)
                throw new Exception("No data provided");       
            var fileId = await _service.CreateFile(dto);
            return CreatedAtAction(nameof(Create), new { id = fileId });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {     
            await _service.DeleteFile(id);
            return NoContent();
        }
    }
}
