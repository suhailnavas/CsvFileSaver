using AutoMapper;
using CsvFileSaver.Models;
using CsvFileSaver.Models.Dto;
using CsvFileSaver.Service;
using CsvFileSaver.Service.IService;
using CsvFileSaver.Utility;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Formats.Asn1;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CsvFileSaver.Controllers
{
    public class UploadFileController : Controller
    {
        private readonly IFileServices _fileService;
        private readonly IMapper _mapper;
        private readonly ILogger<UploadFileController> _logger;
        public UploadFileController(IMapper mapper,IFileServices fileService,ILogger<UploadFileController> logger)
        {
            _fileService = fileService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IActionResult> UploadFile()
        {
            var userRole = HttpContext.Session.GetString(Constants.UserRole);
            var userId = HttpContext.Session.GetString(Constants.UserId);
            var token = HttpContext.Session.GetString(Constants.SessionToken);
            return View( await GetFileData(token, userId, userRole));
        }

        [HttpGet]
        private async Task<List<FileDetailsModel>> GetFileData(string token,string userId, string userRole)
        {
            try
            {
                List<FileDetailsModel> list = new();
                var response = await _fileService.GetAllAsync<APIResponse>(token, userId, userRole);
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<FileDetailsModel>>(Convert.ToString(response.Result));
                }
                return list;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the document data. The issue has been logged for further analysis.");
                return null;
            }         
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            try
            {
                foreach (var item in files)
                {
                    if (item.Length > 0 && Path.GetExtension(item.FileName).ToLower() == ".csv")
                    {
                        //You can optionally save to disk here
                        using var content = new MultipartFormDataContent();
                        using var streamContent = new StreamContent(item.OpenReadStream());
                        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
                        content.Add(streamContent, "file", item.FileName);

                        using var memoryStream = new MemoryStream();
                        await item.CopyToAsync(memoryStream);

                        FileDetailsModel newDocument = new FileDetailsModel
                        {
                            FileName = item.FileName,
                            UserName = HttpContext.Session.GetString(Constants.UserName),
                            UserId = int.Parse(HttpContext.Session.GetString(Constants.UserId)),
                            ContentType = item.ContentType,
                            Content = memoryStream.ToArray(),
                            status = Constants.FileUploaded,
                            IsUpdated = false
                        };
                        var token = HttpContext.Session.GetString(Constants.SessionToken);
                        APIResponse result = await _fileService.SedAsync<APIResponse>(newDocument, token);
                        if (result != null && result.IsSuccess)
                        {
                            return RedirectToAction("UploadFile");
                        }
                    }
                    else
                    {
                        TempData["message"] = "Invalid document. Please upload a valid CSV file";
                    }
                }
                return RedirectToAction("UploadFile"); // Refresh list
            }
            catch(NullReferenceException ex)
            {
                _logger.LogError(ex, "An error occurred while uploading the document . The issue has been logged for further analysis.");
                TempData["message"] = "Invalid document. Please upload a valid CSV file";
                return RedirectToAction("UploadFile");
            }
            catch(Exception e)
            {
                TempData["message"] = e.GetType().ToString();
                return RedirectToAction("UploadFile");
            }
        }

        [HttpPost("UploadRecords")]
        public async Task<IActionResult> UploadRecords(FileDetailsModel selectedFile)
        {
            try
            {
                selectedFile.Content = Convert.FromBase64String(selectedFile.Base64Content);
                var recordList = ReadCsvFromBytes(selectedFile.Content);
                var postRequest = new FilesAndRecordsDto
                {
                    FileDetails = _mapper.Map<FileDetailsDto>(selectedFile),
                    RecordsDetails = _mapper.Map<List<CsvEmployeeRecordDto>>(recordList),                   
                };
                
                var token = HttpContext.Session.GetString(Constants.SessionToken);
                APIResponse result = await _fileService.SedRecorsAsync<APIResponse>(postRequest, token);
                if (result != null && result.IsSuccess)
                {
                    return RedirectToAction("UploadFile");
                }
                return RedirectToAction("UploadFile");
            }
            catch(HeaderValidationException)
            {
                TempData["message"] = "Please check the employee data format in the document.";
                return RedirectToAction("UploadFile");
            }
            catch(Exception e)
            {
                TempData["message"] = e.GetType().ToString();
                return RedirectToAction("UploadFile");
            }
        }

        [HttpGet("GetRecords")]
        public async Task<IActionResult> GetRecords(FileDetailsModel selectedFile)
        {
            try
            {
                var token = HttpContext.Session.GetString(Constants.SessionToken);

                List<CsvEmployeeRecordDto> list = new();
                var response = await _fileService.GetRecordsAsync<APIResponse>(selectedFile.Id,token);
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<CsvEmployeeRecordDto>>(Convert.ToString(response.Result));
                }
                return View(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the document data. The issue has been logged for further analysis.");
                return null;
            }
        }


        public List<CsvEmployeeRecord> ReadCsvFromBytes(byte[] fileBytes)
        {
            using var stream = new MemoryStream(fileBytes);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };

            using var csv = new CsvReader(reader, csvConfig);
            var records = csv.GetRecords<CsvEmployeeRecord>().ToList();

            
            return records;
        }
    }
}
