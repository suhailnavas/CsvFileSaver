using AutoMapper;
using CsvFileSaver.Models;
using CsvFileSaver.Models.Dto;

namespace CsvFileSaver
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<FileDetailsModel, FileDetailsDto>().ReverseMap();
            CreateMap<CsvEmployeeRecordDto, CsvEmployeeRecord>().ReverseMap();
        }
    }
}
