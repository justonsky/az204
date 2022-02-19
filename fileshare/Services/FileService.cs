using System.Threading.Tasks;
using FileShare.Contracts;
using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using FileShare.Services.Interfaces;
using File = FileShare.Models.File;

namespace FileShare.Services
{
    public class FileService : IFileService
    {
        private readonly FileShareContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(
            FileShareContext context, 
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IConfiguration configuration,
            BlobServiceClient blobServiceClient)
        {
            _context = context;
            _mapper = mapper;
            _config = configuration;
            _httpContextAccessor = httpContextAccessor;
            var azureStorageConnString = _config["StorageConnectionString"];
            _blobServiceClient = blobServiceClient;
        }

        public async Task<List<FileDto>> GetFiles(Guid roomId)
        {
            if (roomId == Guid.Empty)
                throw new Exception($"Guid is invalid: {roomId}");
            var room = await _context.Rooms.FindAsync(roomId);
            var files = room.Files?.ToArray();
            return _mapper.Map<File[], List<FileDto>>(files);
        }

        public async Task<FileDto> GetFileInfo(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception($"Guid is invalid: {id}");
            var file = await _context.Files.FindAsync(id);
            return _mapper.Map<File, FileDto>(file);
        }

        public async Task<BinaryData> GetFileDownload(Guid roomId, string fileName)
        {
            if (roomId == Guid.Empty)
                throw new Exception($"Room Guid is invalid: {roomId}");
            var containerClient = _blobServiceClient
                .GetBlobContainerClient(roomId.ToString());
            var blobClient = containerClient.GetBlobClient(fileName);
            var download = await blobClient.DownloadContentAsync();
            return download.Value.Content;
        }

        public async Task<Guid> CreateFile(CreateFileDto dto)
        {
            if (dto == null || dto.File == null)
                throw new Exception($"No file provided");
            var file = _mapper.Map<File>(dto);
            file.Id = Guid.NewGuid();
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
            var containerClient = _blobServiceClient
                .GetBlobContainerClient(dto.RoomId.ToString());
            var blobClient = containerClient.GetBlobClient(file.Name);
            using var fileStream = dto.File.OpenReadStream();
            await blobClient.UploadAsync(fileStream, true);
            return file.Id;
        }

        public async Task DeleteFile(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception($"Guid is invalid: {id}");
            var file = await _context.Files.FindAsync(id);
            _context.Remove(file);
            await _context.SaveChangesAsync();
        }
    }
}