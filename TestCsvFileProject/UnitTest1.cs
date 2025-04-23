using CsvFileSaver_WebApi.Controllers.V1;
using CsvFileSaver_WebApi.Models.Dto;
using CsvFileSaver_WebApi.Repository.IRepository;
using Moq;

namespace TestCsvFileProject
{
    public class UnitTest1
    {
        [Fact]
        public void TestFileUpload()
        {
            var MoqInterface = new Mock<IUserRepository>();
            var Moqcollection = new Mock<FileUploadController>();
            var dtoiput = new LoginRequestDto{ Email = "suhail@gmail.com", Password = "123123" };
            var dtooutput = new LoginResponceDto{ Email = "suhail@gmail.com",};

            MoqInterface.Setup(repo => repo.Login(dtoiput)).Returns(Task.FromResult(dtooutput));
        }
    }
}