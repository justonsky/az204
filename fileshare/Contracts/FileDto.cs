using System;

namespace FileShare.Contracts
{
    public class FileDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public string Name { get; set; }
    }
}