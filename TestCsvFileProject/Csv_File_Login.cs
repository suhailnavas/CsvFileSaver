using AutoMapper;
using CsvFileSaver_WebApi.Controllers.V1;
using CsvFileSaver_WebApi.Model;
using CsvFileSaver_WebApi.Models.Dto;
using CsvFileSaver_WebApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCsvFileProject
{
    public class Csv_File_Login
    {
        [Fact]
        public void Login_Test()
        {
            var Ilogin = new Mock<ILoginRepository>();
            var Iuser = new Mock<IUserRepository>();
            var IMap = new Mock<IMapper>();
            var Controller = new LoginController(Ilogin.Object,IMap.Object,Iuser.Object);

            var loginRequestDto = new LoginRequestDto
            {
                Email = "suhail@gmail.com",
                Password = "123123"
            };

            var responce = new LoginResponceDto
            {
                Id = 1,
                Email = "suhail@gmail.com",
                Name = "suhail",
                Role = "User"
            };

            var log = Iuser.Setup(repo => repo.Login(loginRequestDto)).Returns(Task.FromResult(responce));

            var result = Controller.Login(loginRequestDto);

            var apiResponce = Assert.IsType<Task<ActionResult<APIResponse>>>(result);

            var response = Assert.IsType<OkObjectResult>(apiResponce.Result.Result);
            var apiResponse = Assert.IsType<APIResponse>(response.Value);
            var finalResponce = Assert.IsType<LoginResponceDto>(apiResponse.Result);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(loginRequestDto.Email, finalResponce.Email);
            Assert.Equal(responce.Role, finalResponce.Role);
        }
    }
}
