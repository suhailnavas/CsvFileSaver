using AutoMapper;
using CsvFileSaver_WebApi.Controllers.V1;
using CsvFileSaver_WebApi.Model;
using CsvFileSaver_WebApi.Models;
using CsvFileSaver_WebApi.Models.Dto;
using CsvFileSaver_WebApi.Repository.IRepository;
using CsvFileSaver_WebApi.Utility;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace TestCsvFileProject
{
    public class Csv_DocumentUpload
    {
        [Fact]
        public void File_Upload_Test()
        {
            var Ifile = new Mock<IFileRepository>();
            var Ilog = new Mock<ILoginRepository>();
            var Imap = new Mock<IMapper>();
            var redis = new Mock<IRedisCacheService>();
            var csvFileDto = new FileDetailsDto
            {
                UserId = 12,
                UserName = "suhail",
                FileName = "sample.csv",
                status = "sample",
                ContentType = "csv",
                Content = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF },
                IsUpdated = false
            };

            var csvFile = new FileDetails
            {
                UserId = 12,
                UserName = "suhail",
                FileName = "sample.csv",
                status = "sample",
                ContentType = "csv",
                Content = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF },
                IsUpdated = false
            };

            var Controller = new FileUploadController(Imap.Object, Ilog.Object, Ifile.Object, redis.Object);
            var ls = Ifile.Setup(repo => repo.FileDetailsUpload(csvFileDto)).Returns(Task.FromResult(csvFileDto));

            var Result = Controller.FileUpload(csvFileDto);
            var responce = Assert.IsType<ActionResult<APIResponse>>(Result.Result);

            var response = Assert.IsType<OkObjectResult>(responce.Result);
            var apiResponse = Assert.IsType<APIResponse>(response.Value);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, apiResponse.StatusCode);
            Assert.True(apiResponse.IsSuccess);
        }       
    }
}
