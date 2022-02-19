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
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _service;
        private readonly ILogger<RoomController> _logger;

        public RoomController(
            ILogger<RoomController> logger, IRoomService service)
        {
            _logger = logger;
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<RoomDto>> Get()
        {
            return await _service.GetRooms();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception("No ID provided");          
            var result = await _service.GetRoom(id);
            if (!User.HasClaim(ClaimTypes.Name, id.ToString()))
                return BadRequest("User has no permissions to delete");
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomDto dto)
        {
            if (dto == null)
                throw new Exception("No data provided");       
            var roomId = await _service.CreateRoom(dto);
            return CreatedAtAction(nameof(Create), new { id = roomId });
        }

        [AllowAnonymous]
        [HttpPost("{id}/login")]
        public async Task<IActionResult> Login(Guid id, [FromForm] string password)
        {
            var room = await _service.GetRoom(id);
            if (!await _service.CheckRoomPassword(id, password))
                return Forbid();
            
            var existingClaims = User.Claims;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, room.Id.ToString()),
                new Claim(ClaimTypes.Role, "User")
            };
            foreach(var claim in existingClaims)
                claims.Add(claim);
            var claimsIdentity = new ClaimsIdentity(claims, 
                CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal( claimsIdentity ),
                new AuthenticationProperties { IsPersistent = true });
            return Redirect(id.ToString());
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {     
            if (!User.HasClaim(ClaimTypes.Name, id.ToString()))
                return BadRequest("User has no permissions to delete");
            await _service.DeleteRoom(id);
            return NoContent();
        }
    }
}
