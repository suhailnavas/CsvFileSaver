using CsvFileSaver_WebApi.Models;
using CsvFileSaver_WebApi.Models.Dto;

namespace CsvFileSaver_WebApi.Repository.IRepository
{
    public interface IFileRepository
    {
        Task<FileDetailsDto> FileDetailsUpload(FileDetailsDto fileDetails);
        Task <List<FileDetails>>GetFileDetails();
    }
}
