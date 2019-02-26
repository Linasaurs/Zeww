using System;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Zeww.Repository;
using Newtonsoft.Json;
using Zeww.Models;


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
            var fileToDownload = _unitOfWork.Files.Get().Where(f => f.name == filename).FirstOrDefault();
            WebClient client = new WebClient();
            var DownloadedFileName = fileToDownload.name + fileToDownload.Extension;
            client.DownloadFile(fileToDownload.source, (pathDownload +"/"+ DownloadedFileName));
        }

        [HttpGet("profile/{id}")]
        public string viewProfile(int id)
        {
            var user = _unitOfWork.Users.Get().Where(u => u.Id == id).FirstOrDefault();
            string userJson = JsonConvert.SerializeObject(user);
            return userJson;
        }
        [HttpPut("EditProfile")]
        public void EditProfile([FromBody] User user)
        {
            var userToEdit = _unitOfWork.Users.Get().Where(u => u.Id == user.Id).FirstOrDefault();
            userToEdit.Name = user.Name;
            userToEdit.UserName = user.UserName;
            userToEdit.Email = user.Email;
            userToEdit.Password = user.Password;
            userToEdit.PhoneNumber = user.PhoneNumber;
            userToEdit.Status = user.Status;
            userToEdit.UserWorkspaces = user.UserWorkspaces;
            userToEdit.UserChats = user.UserChats;
            _unitOfWork.Users.Update(userToEdit);
            _unitOfWork.Save();
        }

        [HttpDelete("LeaveChannel/{userId}/{channelId}")]
        public void LeaveChannel(int channelId, int userId)
        {
            //var userDeleteing = _unitOfWork.Users.Get().Where(u => u.Id == userId).FirstOrDefault();
            var chatToBeDeleted = _unitOfWork.Chats.Get().Where(c => c.Id == channelId).FirstOrDefault();
            var userChannel = _unitOfWork.UserChats.Get().Where(c => (c.ChatId == channelId) && ( c.UserId == userId)).FirstOrDefault();
            if (!chatToBeDeleted.IsPrivate)
            {
                _unitOfWork.UserChats.Delete(userChannel);
                _unitOfWork.Save();
            }
        }

    }
}
