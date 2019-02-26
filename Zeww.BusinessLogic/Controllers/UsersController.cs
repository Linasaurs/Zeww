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
   
        public UsersController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        //Downloading to local device using url from database
        public static string getHomePath()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
                return Environment.GetEnvironmentVariable("HOME");

            return Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }

        [HttpGet("download/{filename}")]
        public void DownloadFile(string filename)
        {
            string pathDownload = Path.Combine(getHomePath(), "Downloads");
            var fileToDownload = _unitOfWork.Files.Get().Where(f => f.Name == filename).FirstOrDefault();
            WebClient client = new WebClient();
            var DownloadedFileName = fileToDownload.Name + fileToDownload.Extension;
            client.DownloadFile(fileToDownload.Source, (pathDownload + "/" + DownloadedFileName));
        }

        // POST api/users
        [HttpPost("Post")]
        public void Post([FromBody] User user) {
            _unitOfWork.Users.Add(user);
            _unitOfWork.Save();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
