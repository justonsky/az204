using System.Threading.Tasks;
using FileShare.Contracts;
using System;
using System.Collections.Generic;
using FileShare.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using FileShare.Extensions;

namespace FileShare.Services
{
    public class UserService : IUserService
    {
        private readonly FileShareContext _fileShareContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            FileShareContext context, IHttpContextAccessor httpContextAccessor)
        {
            _fileShareContext = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool UserHasPermissionsForRoom(Guid roomId)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var sessionRooms = httpContext.Session.Get<List<Guid>>(Constants.SessionCookieName);
            if (sessionRooms == null)
                return false;
            return sessionRooms.Contains(roomId);
        }

        public void AddRoomToUser(Guid roomId)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var sessionRooms = httpContext.Session.Get<List<Guid>>(Constants.SessionCookieName);
            if (sessionRooms == null)
                sessionRooms = new List<Guid>();
            if (!sessionRooms.Contains(roomId))
            {
                sessionRooms.Add(roomId);
                httpContext.Session.Set(Constants.SessionCookieName, sessionRooms);
            }
        }
    }
}