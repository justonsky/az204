using System.Threading.Tasks;
using FileShare.Contracts;
using System;
using System.Collections.Generic;

namespace FileShare.Services.Interfaces
{
    public interface IUserService
    {
        bool UserHasPermissionsForRoom(Guid roomId);
        void AddRoomToUser(Guid roomId);
    }
}