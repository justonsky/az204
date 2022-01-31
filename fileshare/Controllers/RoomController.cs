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

        [HttpGet]
        public async Task<IEnumerable<RoomDto>> Get()
        {
            return await _service.GetRooms();
        }

        [HttpGet("{id}")]
        public async Task<RoomDto> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception("No ID provided");          
            return await _service.GetRoom(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomDto dto)
        {
            if (dto == null)
                throw new Exception("No data provided");       
            var roomId = await _service.CreateRoom(dto);
            return CreatedAtAction(nameof(Create), new { id = roomId });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {     
            await _service.DeleteRoom(id);
            return NoContent();
        }
    }
}
