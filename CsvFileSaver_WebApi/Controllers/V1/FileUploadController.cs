using AutoMapper;
using Azure;
using CsvFileSaver.Models;
using CsvFileSaver_WebApi.Model;
using CsvFileSaver_WebApi.Models;
using CsvFileSaver_WebApi.Models.Dto;
using CsvFileSaver_WebApi.Repository.IRepository;
using CsvFileSaver_WebApi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
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
        private readonly IRedisCacheService _cache;
        public FileUploadController(IMapper mapper,ILoginRepository dbLogin,
            IFileRepository fileRepo, IRedisCacheService cache)
        {
            _response = new APIResponse();
            _fileRepo = fileRepo;
            _mapper = mapper;
            _cache = cache;
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
                    var responce = await _fileRepo.FileDetailsUpload(file);
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
                        await _cache?.RemoveAsync("all_products");
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
        public async Task<ActionResult<APIResponse>> GetFiles([FromQuery] string role, [FromQuery]string userId)
        {
            try
            {
                    var responce = await GetFileWithCaching(role, userId);
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

        private async Task<object> GetFileWithCaching(string role, string userId)
        {
            const string cacheKey = "all_products";
            var cachedData = await _cache.GetAsync<List<FileDetailsDto>>(cacheKey);

            if (cachedData != null)
                return ResultFilter(cachedData,role,userId);

            var responce = _mapper.Map<List<FileDetailsDto>>(await _fileRepo.GetFileDetails()); // Assume this is from DB
            await _cache.SetAsync(cacheKey, responce, TimeSpan.FromMinutes(10));
            return ResultFilter(responce, role, userId);
        }

        private object ResultFilter(List<FileDetailsDto> responce, string role, string userId)
        {
            if (role == "Admin")
                return responce;
            else
            {
                return responce.Where(p => p.UserId == int.Parse(userId));
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("UploadRecords")]
        public async Task<ActionResult<APIResponse>> UploadRecords(FilesAndRecordsDto filesAndRecords)
        {
            try
            {
                if (filesAndRecords == null || filesAndRecords.FileDetails ==null || filesAndRecords.RecordsDetails==null)
                {
                    return BadRequest();
                }
                else
                {
                    var csvRecords  =_mapper.Map<List<CsvEmployeeRecord>>(filesAndRecords.RecordsDetails);
                    
                    var recordResponce = await _fileRepo.UploadRecords(csvRecords);
                    if (recordResponce == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.ErrorMessages.Add("Records Not saved");
                        return NotFound();
                    }
                    else
                    {
                        filesAndRecords.FileDetails.status = Constants.RecordsUplaoded;
                        filesAndRecords.FileDetails.IsUpdated = true;
                        var fileResponce = await UpdateFileStatus(filesAndRecords.FileDetails);
                        if(_response.IsSuccess)
                        {
                            _response.Result = new FilesAndRecordsDto
                            {
                                FileDetails = fileResponce,
                                RecordsDetails = _mapper.Map<List<CsvEmployeeRecordDto>>(recordResponce)
                            };
                            _response.StatusCode = HttpStatusCode.OK;
                            return Ok(_response);
                        }
                        else
                        {
                            _response.StatusCode = HttpStatusCode.NotFound;
                            _response.ErrorMessages.Add("File Not saved");
                            return NotFound();
                        }                  
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

        private async Task<FileDetailsDto> UpdateFileStatus(FileDetailsDto file)
        {
            var csvFile = new FileDetailsDto
            {
                Id = file.Id,
                UserId =file.UserId,
                UserName = file.UserName,
                FileName = file.FileName,
                ContentType = file.ContentType,
                Content = file.Content,
                status = file.status,
                IsUpdated = file.IsUpdated
            };

            var responce = await _fileRepo.UpdateFileDetails(csvFile);
            if (responce == null)
            {
                _response.IsSuccess = false;
                return responce;
            }
            else
            {
                _response.IsSuccess = true;
                await _cache?.RemoveAsync("all_products");
                return responce;
            }
        }
    }
}

