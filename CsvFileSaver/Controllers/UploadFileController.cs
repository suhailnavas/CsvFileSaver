using CsvFileSaver.Models;
using CsvFileSaver.Service;
using CsvFileSaver.Service.IService;
using CsvFileSaver.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace CsvFileSaver.Controllers
{
    public class UploadFileController : Controller
    {
        private readonly IFileServices _fileService;
        public UploadFileController(IFileServices fileService)
        {
            _fileService = fileService;
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
                        status = Constants.Status.Not_Updated.ToString(),
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
                    TempData["message"] = "Please select CSV file";
                }
            }
            return RedirectToAction("UploadFile"); // Refresh list
        }

        [HttpPost("UpdateRecords")]
        public IActionResult UpdateRecords(FileDetailsModel selectedFile)
        {          
            return View();
        }
    }
}
