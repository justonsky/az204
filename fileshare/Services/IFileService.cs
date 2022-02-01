using System.Threading.Tasks;
using FileShare.Contracts;
using System;
using System.Collections.Generic;

namespace FileShare.Services.Interfaces
{
    public interface IFileService
    {
        Task<List<FileDto>> GetFiles(Guid roomId);
        Task<FileDto> GetFileInfo(Guid id);
        Task<BinaryData> GetFileDownload(Guid roomId, string fileName);
        Task<Guid> CreateFile(CreateFileDto dto);
        Task DeleteFile(Guid id);
    }
}