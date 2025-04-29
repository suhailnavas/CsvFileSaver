using AutoMapper;
using Azure;
using CsvFileSaver.Models;
using CsvFileSaver_WebApi.Controllers.V1;
using CsvFileSaver_WebApi.Model;
using CsvFileSaver_WebApi.Models;
using CsvFileSaver_WebApi.Models.Dto;
using CsvFileSaver_WebApi.Repository.IRepository;
using CsvFileSaver_WebApi.Utility;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace TestCsvFileProject
{
    public class Csv_FileTest
    {
        [Fact]
        public void Register_Test()
        {
            var IUser = new Mock<IUserRepository>();
            var Ilogin = new Mock<ILoginRepository>();
            var IMap = new Mock<IMapper>();

            var recordDto = new RegisterationRequestDTO
            {             
                Name = "suhail",
                Email ="suhail@gmail.com",
                Password ="123123",
                Role= "Admin"
            };
            var record = new RegisterationRequest
            {
                Name = "suhail",
                Email = "suhail@gmail.com",
                Password = "123123",
                Role = "Admin"
            };
            IUser.Setup(repo => repo.Register(recordDto)).Returns(Task.FromResult(record));
            IUser.Setup(repo => repo.IsUniqueUser(recordDto.Email)).Returns(true);
            var cnt = new LoginController(Ilogin.Object, IMap.Object, IUser.Object);
        
            var _response = new APIResponse();
            var actionResult =cnt.Register(recordDto);

            var badRequest = Assert.IsType<Task<IActionResult>>(actionResult);

            var response = Assert.IsType<OkObjectResult>(badRequest.Result);
            var apiResponse = Assert.IsType<APIResponse>(response.Value);

            Assert.Equal(200,response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, apiResponse.StatusCode);
            Assert.True(apiResponse.IsSuccess);
        }
    }
}