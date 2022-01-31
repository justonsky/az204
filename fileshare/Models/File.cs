using System;
using System.ComponentModel.DataAnnotations;

namespace FileShare.Models
{
    public class File
    {
        public Guid Id { get; set; }
        [Required]
        public Guid RoomId { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual Room Room { get; set; }
    }
}