using System.Threading.Tasks;
using FileShare.Contracts;
using System;
using System.Collections.Generic;
using FileShare.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using FileShare.Services.Interfaces;

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
            IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _config = configuration;
            _httpContextAccessor = httpContextAccessor;
            var azureStorageConnString = _config["StorageConnectionString"];
            _blobServiceClient = new BlobServiceClient(azureStorageConnString);
        }

        public async Task<List<FileDto>> GetFiles(Guid roomId)
        {
            if (roomId == Guid.Empty)
                throw new Exception($"Guid is invalid: {roomId}");
            var room = await _context.Rooms.FindAsync(roomId);
            var files = room.Files.ToArray();
            return _mapper.Map<File[], List<FileDto>>(files);
        }

        public async Task<FileDto> GetFileInfo(Guid id)
        {
            if (id == Guid.Empty)
                throw new Exception($"Guid is invalid: {id}");
            var file = await _context.Files.FindAsync(id);
            return _mapper.Map<File, FileDto>(file);
        }

        public async Task<Guid> CreateFile(CreateFileDto dto)
        {
            var file = _mapper.Map<File>(dto);
            file.Id = Guid.NewGuid();
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
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