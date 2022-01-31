using AutoMapper;
using FileShare.Models;
using FileShare.Contracts;
namespace FileShare.AutoMapper
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
            CreateMap<File, FileDto>();
            CreateMap<CreateFileDto, File>()
                .ForMember(dest => dest.Name, 
                    dest => dest.MapFrom(source => source.File.FileName));
        }
    }
}