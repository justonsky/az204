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
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly ILogger<FileController> _logger;

        public FileController(
            ILogger<FileController> logger, 
            IFileService fileService,
            IUserService userService)
        {
            _logger = logger;
            _fileService = fileService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid roomId)
        {
            if (roomId == Guid.Empty || !_userService.UserHasPermissionsForRoom(roomId))
                return BadRequest("User does not have appropriate permissions");
            return Ok(await _fileService.GetFiles(roomId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("File ID is invalid");
            var fileInfo = await _fileService.GetFileInfo(id);
            if (!_userService.UserHasPermissionsForRoom(fileInfo.RoomId))
                return BadRequest("User does not have appropriate permissions");
            return Ok(fileInfo);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("File ID is invalid");
            var fileInfo = await _fileService.GetFileInfo(id);
            if (!_userService.UserHasPermissionsForRoom(fileInfo.RoomId))
                return BadRequest("User does not have appropriate permissions"); 
            var results = await _fileService.GetFileDownload(fileInfo.RoomId, fileInfo.Name);
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
            var fileId = await _fileService.CreateFile(dto);
            return CreatedAtAction(nameof(Create), new { id = fileId });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {   
            if (id == Guid.Empty)
                return BadRequest("File ID is invalid");
            var fileInfo = await _fileService.GetFileInfo(id);
            if (!_userService.UserHasPermissionsForRoom(fileInfo.RoomId))
                return BadRequest("User does not have appropriate permissions");
            await _fileService.DeleteFile(id);
            return NoContent();
        }
    }
}
