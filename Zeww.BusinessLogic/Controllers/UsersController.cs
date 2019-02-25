using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zeww.DAL;
using Zeww.Models;
using Zeww.Repository;
using static System.Net.Mime.MediaTypeNames;
using File = Zeww.Models.File;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zeww.BusinessLogic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IUnitOfWork _unitOfWork;
        //IHostingEnvironment _env;
        //private readonly IHostingEnvironment _hostingEnvironment;

        public UsersController(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment /*IHostingEnvironment env*/)
        {
            //_hostingEnvironment = hostingEnvironment;
            //this._env = env;
            this._unitOfWork = unitOfWork;
        }

        // GET: /<controller>/
        //public string Index()
        //{
        //    return "Hello";
        //}

        //[HttpGet("{id}")]
        //public string GetById(int Id) {
        //    return _unitOfWork.Users.GetByID(Id).Name;
        //}

        //// POST api/users
        //[HttpPost]
        //public void Post([FromBody] User user)
        //{
        //    _unitOfWork.Users.Add(user);
        //    _unitOfWork.Save();
        //}

        //[HttpPost]
        ////[Route("~/upload")]
        //public async Task<IActionResult> UploadFile([FromBody]File model)
        //{
        //    var file = model.file;
        //    if (file.Length > 0)
        //    {
        //        string path = Path.Combine(_env.WebRootPath, "uploadFiles");
        //        using (var fs = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
        //        {
        //            await file.CopyToAsync(fs);
        //        }
        //        model.source = $"/uploadFiles{file.FileName}";
        //        model.Extension = Path.GetExtension(file.FileName).Substring(1);
        //    }
        //    return BadRequest();
        //}
        //("UploadFiles")
        //    [HttpPost]
        //    [Route("upload")]
        //    [Produces("application/json")]
        //    public async Task<IActionResult> uploadFile(List<IFormFile> files)
        //    {
        //        // Get the file from the POST request
        //        var theFile = HttpContext.Request.Form.Files.GetFile("file");

        //        // Get the server path, wwwroot
        //        string webRootPath = _hostingEnvironment.WebRootPath;

        //        // Building the path to the uploads directory
        //        var fileRoute = Path.Combine(webRootPath, "uploads");

        //        // Get the mime type
        //        var mimeType = HttpContext.Request.Form.Files.GetFile("file").ContentType;

        //        // Get File Extension
        //        string extension = Path.GetExtension(theFile.FileName);

        //        // Generate Random name.
        //        string name = Guid.NewGuid().ToString().Substring(0, 8) + extension;

        //        // Build the full path inclunding the file name
        //        string link = Path.Combine(fileRoute, name);

        //        // Create directory if it dose not exist.
        //        FileInfo dir = new FileInfo(fileRoute);
        //        dir.Directory.Create();

        //        // Basic validation on mime types and file extension
        //        string[] imageMimetypes = { "image/gif", "image/jpeg", "image/pjpeg", "image/x-png", "image/png", "image/svg+xml" };
        //        string[] imageExt = { ".gif", ".jpeg", ".jpg", ".png", ".svg", ".blob" };

        //        try
        //        {
        //            if (Array.IndexOf(imageMimetypes, mimeType) >= 0 && (Array.IndexOf(imageExt, extension) >= 0))
        //            {
        //                // Copy contents to memory stream.
        //                Stream stream;
        //                stream = new MemoryStream();
        //                theFile.CopyTo(stream);
        //                stream.Position = 0;
        //                String serverPath = link;

        //                // Save the file
        //                using (FileStream writerFileStream = System.IO.File.Create(serverPath))
        //                {
        //                    await stream.CopyToAsync(writerFileStream);
        //                    writerFileStream.Dispose();
        //                }

        //                // Return the file path as json
        //                Hashtable imageUrl = new Hashtable();
        //                imageUrl.Add("link", "/uploads/" + name);

        //                return Json(imageUrl);
        //            }
        //            throw new ArgumentException("The image did not pass the validation");
        //        }
        //        catch (ArgumentException ex)
        //        {
        //            return Json(ex.Message);
        //        }
        //    }
        //}

        [HttpGet]
        [Route("download")]
        public void downloadFile(List<IFormFile> files)
        {
            try
            {
                string remoteUri = "https://cdn.pixabay.com/photo/2017/04/20/17/09/letter-e-2246323_960_720.png";
                string fileName = "test", myStringWebResource = null;
                WebClient myWebClient = new WebClient();
                myStringWebResource = remoteUri + fileName;
                myWebClient.DownloadFile(myStringWebResource, fileName);
            }
            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message);
            }
        }
    }
}
