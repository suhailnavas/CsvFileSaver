using AutoMapper;
using CsvFileSaver_WebApi.Data;
using CsvFileSaver_WebApi.Models;
using CsvFileSaver_WebApi.Models.Dto;
using CsvFileSaver_WebApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CsvFileSaver_WebApi.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public FileRepository(ApplicationDbContext db, IConfiguration configuration, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<FileDetailsDto> FileDetailsUpload(FileDetailsDto fileDetails)
        {
            FileDetails newFileDetails = new()
            {
                FileName = fileDetails.FileName,
                Content = fileDetails.Content,
                ContentType = fileDetails.ContentType,
                status = fileDetails.status,
                IsUpdated =fileDetails.IsUpdated
            };
            _db.UploadFileDetails.Add(newFileDetails);
            await _db.SaveChangesAsync();
            return fileDetails;
        }
        
        public async Task<List<CsvEmployeeRecord>> UploadRecords(List<CsvEmployeeRecord> recordDetails)
        {
            //_db.CsvEmployeeRecords.AddRange(recordDetails);
            //await _db.SaveChangesAsync();

            var batchSize = 1000;  // Insert records in batches of 1000
            for (int i = 0; i < recordDetails.Count; i += batchSize)
            {
                var batch = recordDetails.Skip(i).Take(batchSize).ToList();
                _db.CsvEmployeeRecords.AddRange(batch);
                await _db.SaveChangesAsync();
            }
            return recordDetails;
        }



        public async Task<List<FileDetails>> GetFileDetails()
        {
            return await _db.UploadFileDetails.ToListAsync();
        }

        public async Task<FileDetailsDto> UpdateFileDetails(FileDetailsDto fileDetails)
        {
            FileDetails newFileDetails = new()
            {
                Id = fileDetails.Id,
                FileName = fileDetails.FileName,
                Content = fileDetails.Content,
                ContentType = fileDetails.ContentType,
                status = fileDetails.status,
                IsUpdated = fileDetails.IsUpdated
            };
            _db.UploadFileDetails.Update(newFileDetails);
            await _db.SaveChangesAsync();
            return fileDetails;
        }
    }
}
