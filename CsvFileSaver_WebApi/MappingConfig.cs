using AutoMapper;
using CsvFileSaver_WebApi.Models;
using CsvFileSaver_WebApi.Models.Dto;

namespace CsvFileSaver_WebApi
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<LoginDto,Login>().ReverseMap();
            CreateMap<RegisterationRequestDTO,RegisterationRequest>().ReverseMap();
            CreateMap<FileDetails,FileDetailsDto>().ReverseMap();
        }
    }
}
