using AutoMapper;
using CsvFileSaver_WebApi.Model;
using CsvFileSaver_WebApi.Models.Dto;
using CsvFileSaver_WebApi.Repository.IRepository;
using CsvFileSaver_WebApi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CsvFileSaver_WebApi.Controllers.V1
{
    [Route(Constants.FileUploadControllerRute)]
    [ApiController]
    [ApiVersion(Constants.ApiVersionOnePointZero)]
    [Authorize]
    public class FileUploadController : Controller
    {
        private readonly APIResponse _response;
        private readonly IFileRepository _fileRepo;
        private readonly IMapper _mapper;
        public FileUploadController(IMapper mapper,ILoginRepository dbLogin, IFileRepository fileRepo)
        {
            _response = new APIResponse();
            _fileRepo = fileRepo;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("FileUploade")]
        public async Task<ActionResult<APIResponse>> FileUpload(FileDetailsDto file)
        {
            try
            {
                if (file == null)
                {
                    return BadRequest();
                }
                else
                {
                    var csvFile = new FileDetailsDto
                    {
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        Content = file.Content
                    };

                    var responce = await _fileRepo.FileDetailsUpload(csvFile);
                    if (responce == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.ErrorMessages.Add("File Not saved");
                        return NotFound();
                    }
                    else
                    {
                        _response.Result = responce;
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        return Ok(_response);
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetFiles")]
        public async Task<ActionResult<APIResponse>> GetFiles()
        {
            try
            {

                    var responce = _mapper.Map<List<FileDetailsDto>>(await _fileRepo.GetFileDetails());
                    if (responce == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.ErrorMessages.Add("File Not saved");
                        return NotFound();
                    }
                    else
                    {
                        _response.Result = responce;
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        return Ok(_response);
                    }
                
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}

