using System.Threading.Tasks;
using System.Collections.Generic;
using FileShare.Contracts;
using System;

namespace FileShare.Services.Interfaces
{
    public interface IRoomService
    {
        Task<List<RoomDto>> GetRooms();
        Task<RoomDto> GetRoom(Guid id);
        Task<Guid> CreateRoom(CreateRoomDto dto);
        Task<bool> CheckRoomPassword(Guid id, string password);
        Task DeleteRoom(Guid id);
    }
}