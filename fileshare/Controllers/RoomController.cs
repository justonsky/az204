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
using Microsoft.AspNetCore.Http;
using FileShare.Extensions;

namespace FileShare.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;
        private readonly ILogger<RoomController> _logger;

        public RoomController(
            ILogger<RoomController> logger, 
            IRoomService roomService,
            IUserService userService)
        {
            _logger = logger;
            _roomService = roomService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<RoomDto>> Get()
        {
            return await _roomService.GetRooms();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception("No ID provided");          
            var result = await _roomService.GetRoom(id);
            if (!_userService.UserHasPermissionsForRoom(id))
                return BadRequest("User has no permissions to view");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomDto dto)
        {
            if (dto == null)
                throw new Exception("No data provided");       
            var roomId = await _roomService.CreateRoom(dto);
            return CreatedAtAction(nameof(Create), new { id = roomId });
        }

        [HttpPost("{id}/login")]
        public async Task<IActionResult> Login(Guid id, [FromForm] string password)
        {
            var room = await _roomService.GetRoom(id);
            if (!await _roomService.CheckRoomPassword(id, password))
                return Forbid();
            _userService.AddRoomToUser(room.Id);
            return Redirect(id.ToString());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {     
            if (!_userService.UserHasPermissionsForRoom(id))
                return BadRequest("User has no permissions to delete");
            await _roomService.DeleteRoom(id);
            return NoContent();
        }
    }
}
