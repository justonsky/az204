using System.Threading.Tasks;
using System.Collections.Generic;
using FileShare.Contracts;
using System;
using FileShare.Services.Interfaces;
using FileShare.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BCrypt.Net;

namespace FileShare.Services
{
    public class RoomService : IRoomService
    {
        private readonly FileShareContext _context;
        private readonly IMapper _mapper;

        public RoomService(FileShareContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    
        public async Task<List<RoomDto>> GetRooms()
        {
            var rooms = await _context.Rooms.ToArrayAsync();
            return _mapper.Map<Room[], List<RoomDto>>(rooms);
        }

        public async Task<RoomDto> GetRoom(Guid id)
        {
            var room = await _context.Rooms.FindAsync(id);
            return _mapper.Map<RoomDto>(room);
        }

        public async Task<Guid> CreateRoom(CreateRoomDto dto)
        {
            var room = _mapper.Map<Room>(dto);
            room.Id = Guid.NewGuid();
            room.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room.Id;
        }

        public async Task DeleteRoom(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception($"Guid is invalid: {id}");
            var room = await _context.Rooms.FindAsync(id);
            _context.Remove(room);
            await _context.SaveChangesAsync();
        }
    }
}