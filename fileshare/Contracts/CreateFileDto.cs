using System;
using Microsoft.AspNetCore.Http;
namespace FileShare.Contracts
{
    public class CreateFileDto
    {
        public Guid RoomId { get; set; }
        public IFormFile File { get; set; }
    }
}