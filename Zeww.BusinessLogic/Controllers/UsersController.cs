﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zeww.BusinessLogic.DTOs;
using Zeww.DAL;
using Zeww.Models;
using Zeww.Repository;
using Zeww.BusinessLogic.ExtensionMethods;
using Newtonsoft.Json;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zeww.BusinessLogic.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IUnitOfWork _unitOfWork;


        public UsersController(IUnitOfWork unitOfWork) {
            this._unitOfWork = unitOfWork;
        }

        // GET: /<controller>/
        public string Index()
        {
            return "Hello";
        }

        [HttpGet("{id}")]
        public ActionResult GetById(int Id) {
            if (Id < 1)
            {
                return BadRequest();
            }

          
            if (_unitOfWork.Users.GetByID(Id)  == null)
            {
                return NotFound();
            }

            var user = new
            {
                id = _unitOfWork.Users.GetByID(Id).Id,
                name = _unitOfWork.Users.GetByID(Id).Name,
                email = _unitOfWork.Users.GetByID(Id).Email,
                UserName = _unitOfWork.Users.GetByID(Id).UserName,
                status = _unitOfWork.Users.GetByID(Id).Status,
                phoneNumber = _unitOfWork.Users.GetByID(Id).PhoneNumber
            };
            return Ok(user);

        }

        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp([FromBody] User user)
        {
            var userNameExists = _unitOfWork.Users.GetUserByUserName(user.UserName) == null ? false : true;
            if (userNameExists)
                return BadRequest("This username is already taken.");

            var emailExists = _unitOfWork.Users.GetUserByEmail(user.Email) == null ? false : true;
            if (emailExists)
                return BadRequest("There is an account with this email.");

            if (ModelState.IsValid)
            {
                var passwordHasher = new PasswordHasher<User>();
                user.Password = passwordHasher.HashPassword(user, user.Password);

                _unitOfWork.Users.Insert(user);
                _unitOfWork.Save();

                var location = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(Request).Replace("SignUp", user.Id.ToString());

                return Created(location, user);
            }

            return BadRequest(ModelState);
        }

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
            client.DownloadFile(fileToDownload.Source, (pathDownload +"/"+ DownloadedFileName));
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] UserSignInDTO userSignInDTO)
        {
            var userRepository = _unitOfWork.Users;
            var user = userRepository.GetUserByEmail(userSignInDTO.Email);
            if(user == null)
            {
                return BadRequest("Invalid email/username or password");
            }
            if(userRepository.Authenticate(user, userSignInDTO.Password))
            {
                return Ok(userRepository.GenerateJWTToken(user));
            }
            return BadRequest("Invalid email/username or password");
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
