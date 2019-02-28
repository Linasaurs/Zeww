using System;
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
        public ActionResult GetById(int Id)
        {
            User _ = this.GetAuthenticatedUser();
            if (Id < 1)
            {
                return BadRequest();
            }


            if (_unitOfWork.Users.GetByID(Id) == null)
            {
                return NotFound();
            }
            
            return Ok(_unitOfWork.Users.GetByID(Id));

        }

        [HttpGet("withoutPasswords/{id}")]
        public ActionResult GetByIdWithoutPassword(int Id)
        {
            User _ = this.GetAuthenticatedUser();
            if (Id < 1)
            {
                return BadRequest();
            }


            if (_unitOfWork.Users.GetByID(Id) == null)
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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet]
        [Route("VerifyUserNameIsUnique")]
        public IActionResult VerifyUserNameIsUnique(UserNameDTO dto)
        {
            var userNameExists = _unitOfWork.Users.GetUserByUserName(dto.UserName) == null ? false : true;
            if (userNameExists)
                return BadRequest("This username is already taken.");

            return Ok("You can use this user name");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("VerifyEmailIsUnique")]
        public IActionResult VerifyEmailIsUnique(EmailDTO dto)
        {
            var emailExists = _unitOfWork.Users.GetUserByEmail(dto.Email) == null ? false : true;
            if (emailExists)
                return BadRequest("This email is already taken.");

            return Ok("You can use this email");
        }

        [HttpGet("ShowConnectionStatusForLoggedInUser")]
        public IActionResult ShowConnectionStatus() {
            //Ziad is working on this method, please do not touch it!
            User userToShowConnectionStatusFor = this.GetAuthenticatedUser();
            var connectionStatus = userToShowConnectionStatusFor.ConnectionStatus;
            return Ok(connectionStatus);
        }

        [HttpPut("ToggleUserConnectionStatusForLoggedInUser")]
        public IActionResult ToggleUserConnectionStatus(int userId, ConnectionStatus newConnectionStatus) {
            //Ziad is working on this method, please do not touch it!
            var userToChangeConnectionStatusFor = _unitOfWork.Users.GetByID(userId);
            if (userToChangeConnectionStatusFor.ConnectionStatus == 0) {
                userToChangeConnectionStatusFor.ConnectionStatus = ConnectionStatus.Away;
            } else {
                userToChangeConnectionStatusFor.ConnectionStatus = ConnectionStatus.Active;
            }

            userToChangeConnectionStatusFor.ConnectionStatus = newConnectionStatus;
            return Ok(userToChangeConnectionStatusFor.ConnectionStatus);
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

        public static string getHomePath()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
                return Environment.GetEnvironmentVariable("HOME");

            return Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }

        [AllowAnonymous]
        [HttpGet("download/{filename}")]
        public IActionResult DownloadFile(string filename)
        {
            string pathDownload = Path.Combine(getHomePath(), "Downloads");
            var fileToDownload = _unitOfWork.Files.Get().Where(f => f.Name == filename).FirstOrDefault();

            if (fileToDownload != null)
            {
                WebClient client = new WebClient();
                var DownloadedFileName = fileToDownload.Name + fileToDownload.Extension;
                client.DownloadFile(fileToDownload.Source, (pathDownload + "/" + DownloadedFileName));
                return Ok();
            }
            return NotFound("File not found");
        }

        [AllowAnonymous]
        [HttpGet("profile/{id}")]
        public IActionResult viewProfile(int id)
        {
            var user = _unitOfWork.Users.Get().Where(u => u.Id == id).FirstOrDefault();
            if (user != null)
            {
                string userJson = JsonConvert.SerializeObject(user);
                return Ok(userJson);
            }
            else
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpPut("EditProfile/{id}")]
        public IActionResult EditProfile(int id, [FromBody] User user)
        {
            var userToEdit = _unitOfWork.Users.Get().Where(u => u.Id == id).FirstOrDefault();
            if (userToEdit != null)
            {
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
                return Ok(userToEdit);
            }
            else
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpDelete("LeaveChannel/{userId}/{channelId}")]
        public IActionResult LeaveChannel(int channelId, int userId)
        {
            var chatToBeDeleted = _unitOfWork.Chats.GetByID(channelId);
            var userChannel = _unitOfWork.UserChats.Get().Where(c => (c.ChatId == chatToBeDeleted.Id) && (c.UserId == userId)).FirstOrDefault();
            if (chatToBeDeleted == null || userChannel == null)
            {
                return NotFound("User is not a member in this channel");
            }
            else
            {
                if (!chatToBeDeleted.IsPrivate)
                {
                    _unitOfWork.UserChats.Delete(userChannel);
                    _unitOfWork.Save();
                    return Ok("User left chat");
                }
                else
                {
                    return BadRequest("You can't delete a private message");
                }
            }
        }


        [HttpPut]
        [Route("AddDontDisturbPeriod")]
        public IActionResult AddDontDisturbPeriod([FromBody] DoNotDisturbDTO dto)
        {
            User user = this.GetAuthenticatedUser();

            var from = dto.DoNotDisturbFrom;
            var to = dto.DoNotDisturbTo;

            if (to <= from)
                return BadRequest("The 'to' value can't be less than or equal the 'from' value");

            if (ModelState.IsValid)
            {
                user.DailyDoNotDisturbFrom = from;
                user.DailyDoNotDisturbTo = to;

                _unitOfWork.Save();

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpGet("userDirectChats/{userId}")]
        public IActionResult GetUserDirectChats(int userId)
        {
            List<int> userDirectChats = new List<int>();

            var allUserChatIDs = _unitOfWork.UserChats.Get()
                .Where(a=> a.UserId== userId)
                .Select(a=> a.ChatId)
                .ToList();

            foreach(var chatID in allUserChatIDs)
            {
               if( _unitOfWork.Chats.GetByID(chatID).IsPrivate)
                    userDirectChats.Add(chatID);
            }
            return Ok(userDirectChats);
        }

        [AllowAnonymous]
        [HttpGet("userChannelChats/{userId}")]
        public IActionResult GetUserChannelChats(int userId)
        {
            List<int> userChannelChats = new List<int>();

            var allUserChatIDs = _unitOfWork.UserChats.Get()
                .Where(a => a.UserId == userId)
                .Select(a => a.ChatId)
                .ToList();

            foreach (var chatID in allUserChatIDs)
            {
                if (!_unitOfWork.Chats.GetByID(chatID).IsPrivate)
                    userChannelChats.Add(chatID);
            }
            return Ok(userChannelChats);
        }

    }
}
