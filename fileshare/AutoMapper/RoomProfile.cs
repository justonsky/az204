using AutoMapper;
using FileShare.Models;
using FileShare.Contracts;
namespace FileShare.AutoMapper
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomDto>();
            CreateMap<CreateRoomDto, Room>();
        }
    }
}