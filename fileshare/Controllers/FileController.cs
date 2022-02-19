using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileShare.Services.Interfaces;
using FileShare.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace FileShare.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Get(Guid roomId)
        {
            if (roomId == Guid.Empty || !User.HasClaim(ClaimTypes.Name, roomId.ToString()))
                return BadRequest("User does not have appropriate permissions");
            return Ok(await _service.GetFiles(roomId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("File ID is invalid");
            var result = await _service.GetFileInfo(id);
            if (!User.HasClaim(ClaimTypes.Name, result.RoomId.ToString()))
                return BadRequest("User does not have appropriate permissions");
            return Ok(result);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("File ID is invalid");
            var fileInfo = await _service.GetFileInfo(id);
            if (!User.HasClaim(ClaimTypes.Name, fileInfo.RoomId.ToString()))
                return BadRequest("User does not have appropriate permissions"); 
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
            if (!User.HasClaim(ClaimTypes.Name, dto.RoomId.ToString()))
                return BadRequest("User does not have appropriate permissions");   
            var fileId = await _service.CreateFile(dto);
            return CreatedAtAction(nameof(Create), new { id = fileId });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {   
            if (id == Guid.Empty)
                return BadRequest("File ID is invalid");
            var fileInfo = await _service.GetFileInfo(id);
            if (!User.HasClaim(ClaimTypes.Name, fileInfo.RoomId.ToString()))
                return BadRequest("User does not have appropriate permissions");
            await _service.DeleteFile(id);
            return NoContent();
        }
    }
}
