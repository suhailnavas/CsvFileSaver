using CsvFileSaver.Models;
using Microsoft.AspNetCore.Mvc;

namespace CsvFileSaver.Controllers
{
    public class UploadFileController : Controller
    {
        List<FileDetailsModel> file = new List<FileDetailsModel>();
        public IActionResult UploadFile()
        {
            if (file == null || file.Count ==0)
            GetFileData();  
            
            return View(file);
        }

        private void GetFileData()
        {

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

                    //using var memoryStream = new MemoryStream();
                    //await item.CopyToAsync(memoryStream);

                    //FileDetailsModel newDocument = new FileDetailsModel
                    //{
                    //    FileName = item.FileName,
                    //    ContentType = item.ContentType,
                    //    Content = memoryStream.ToArray()
                    //};
                }
                else
                {
                    TempData["message"] = "Please select CSV file";
                }
            }
            return RedirectToAction("UploadFile", file); // Refresh list
        }

        public IActionResult ShowTable()
        {          
            return View();
        }
    }
}
