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
        public UploadFileController(IMapper mapper,IFileServices fileService)
        {
            _fileService = fileService;
            _mapper = mapper;
        }
        public async Task<IActionResult> UploadFile()
        {                        
            return View( await GetFileData());
        }

        [HttpGet]
        private async Task<List<FileDetailsModel>> GetFileData()
        {
            List<FileDetailsModel> list = new();
            var response = await _fileService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(Constants.SessionToken)); 
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<FileDetailsModel>>(Convert.ToString(response.Result));
            }
            return list;
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
            catch(NullReferenceException)
            {
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
                    RecordsDetails = _mapper.Map<List<CsvEmployeeRecordDto>>(recordList)
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
