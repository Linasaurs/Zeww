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
using System.Net.Mail;


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

            var user = _unitOfWork.Users.GetByID(Id);
        
             var parsedString = Enum.GetName(typeof(Status), (int)user.Status);
            //Enum.TryParse(parsedString, out Enum user.status);
            Status st=(Status) Enum.Parse(typeof(Status), parsedString);
            user.Status = st;
            if (user.Status.ToString() == parsedString)
            {
                return  Ok(Status.Busy.ToString());
            }

            return Ok(parsedString);

        }

        [HttpGet]
        [Route("getEnumStatusName/{user.Id}")]
        public string GetEnumStatusName([FromBody] User user)
        {
            User _user = this.GetAuthenticatedUser();

            var parsedString = Enum.GetName(typeof(Status), (int)user.Status);

            if(_user.Id != user.Id)
            {
                return "You aren't authenticated!";
            }
            else
            {
                if (user.Status.ToString() == parsedString)
                {
                    return parsedString;
                }
                else
                {
                    return "status not found!";
                }

            }
        }


        [HttpPut]
        [Route("UpdateEnumStatusName")]
        public IActionResult UpdateEnumStatusName([FromBody] string status)
        {
            User _user = this.GetAuthenticatedUser();

            Status st = (Status)Enum.Parse(typeof(Status), status);

            _user.Status = st;

            return NoContent();
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

                var location = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(Request).ToLower().Replace("signup", user.Id.ToString());

                return Created(location, user);
            }

            return BadRequest(ModelState);
        }

        //Not Done Yet "Get User Channels"
        [HttpGet]
        [Route("GetChannels")]
        public IActionResult GetUserChannels()
        {
            User user = this.GetAuthenticatedUser();
            //if (user.Id <= 0)
            //{
            //    return BadRequest("ID must be greater than zero");
            //}
            var chat = user.UserChats;
            if (chat == null)
            {
                return NotFound();
            }      
            return Ok(chat);
        }

        [HttpPut]
        [Route("ChangeLanguageRegion")]
        public IActionResult ChangeLanguageRegion([FromBody]LanguageRegionDTO dto)
        {
            User user = this.GetAuthenticatedUser();
            if (user.Language != null && user.Region!=null)
            {
                user.Language = dto.Language;
                user.Region = dto.Region;
               
                _unitOfWork.Users.Update(user);
                _unitOfWork.Save();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("VerifyUserNameIsUnique")]
        public IActionResult VerifyUserNameIsUnique([FromQuery]string userName)
        {
            var userNameExists = _unitOfWork.Users.GetUserByUserName(userName) == null ? false : true;
            if (userNameExists)
                return BadRequest("This username is already taken.");
            return Ok("You can use this user name");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("VerifyEmailIsUnique")]
        public IActionResult VerifyEmailIsUnique([FromQuery]string email)
        {
            var emailExists = _unitOfWork.Users.GetUserByEmail(email) == null ? false : true;
            if (emailExists)
                return BadRequest("This email is already taken.");

            return Ok("You can use this email");
        }

        [HttpGet("ShowConnectionStatusForLoggedInUser")]
        public IActionResult ShowConnectionStatus() {
            User userToShowConnectionStatusFor = this.GetAuthenticatedUser();
            var connectionStatus = userToShowConnectionStatusFor.ConnectionStatus;
            return Ok(connectionStatus);
        }

        [HttpPut("ToggleUserConnectionStatusForLoggedInUser")]
        public IActionResult ToggleUserConnectionStatus(int userId, ConnectionStatus newConnectionStatus) {
            var userToChangeConnectionStatusFor = _unitOfWork.Users.GetByID(userId);
            if (userToChangeConnectionStatusFor.ConnectionStatus == 0) {
                userToChangeConnectionStatusFor.ConnectionStatus = ConnectionStatus.Away;
            } else {
                userToChangeConnectionStatusFor.ConnectionStatus = ConnectionStatus.Active;
            }

            userToChangeConnectionStatusFor.ConnectionStatus = newConnectionStatus;
            return Ok(userToChangeConnectionStatusFor.ConnectionStatus);
         }

        public Status ShowStatusById(int userId) {
            var userToShowStatusFor = _unitOfWork.Users.GetByID(userId);
            var userStatus = userToShowStatusFor.Status;
            return userStatus;
        }

        [HttpGet("ShowStatusToAllUsersInWorkspace")]
        public IActionResult ShowStatusToAllUsersInWorkspace(int[] userIds) {
            var dictionary = new Dictionary<int, Status>();
            var allUsersInCurrentWorkspace = _unitOfWork.Workspaces.GetUsersIdInWorkspace(1);
            //Will have to change this method to be used to get the current workspace
            if (allUsersInCurrentWorkspace == null) {
                return BadRequest("No users in this workspace");
            }

            foreach(int userId in allUsersInCurrentWorkspace) {
                var userStatus = _unitOfWork.Users.GetByID(userId).Status;
                dictionary.Add(userId, userStatus);
            }
            return Ok(dictionary);
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
        [HttpGet("GetworkspacesbyUserId/{id}")]
        public IActionResult GetworkspacesbyUserId(int id)
        {
            var user = _unitOfWork.Users.GetWorkspaceIdsByUserId(id);
            if (user != null)
            {
                List<Workspace> workspaces = new List<Workspace>();
                foreach(var workspace in user)
                {
                    workspaces.Add(_unitOfWork.Workspaces.GetByID(workspace));
                }
                string userJson = JsonConvert.SerializeObject(workspaces);
                return Ok(userJson);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("SearchByUserName/{userName}")]
        public IActionResult SearchByUserName(string userName, int workspaceId) {
            var listOfUsersIdsInWorkspace = _unitOfWork.Workspaces.GetUsersIdInWorkspace(workspaceId);
            if (listOfUsersIdsInWorkspace == null) {
                return Ok("No users in passed workspace");
            }
            var listOfUsersInWorkspace = new List<User>();
            foreach (int userId in listOfUsersIdsInWorkspace) {
                listOfUsersInWorkspace.Add(_unitOfWork.Users.GetByID(userId));
            }
            return Ok(listOfUsersInWorkspace);
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

        [HttpPut]
        [Route("AddCustomStatus")]
        public IActionResult AddCustomStatus([FromBody] CustomStatusDTO CustomStatus)
        {
            User user = this.GetAuthenticatedUser();
            if (CustomStatus.status == null)
            {
                return BadRequest("Invalid Request");
            }
            switch (CustomStatus.status.ToLower())
            {
                case "available":
                    user.Status = Status.Available;
                    break;
                case "busy":
                    user.Status = Status.Busy;
                    break;
                case "away":
                    user.Status = Status.Away;
                    break;
                case "customstatus":
                    if (CustomStatus.customStatus == null) {
                        return BadRequest("Invalid Custom Status");
                    }
                    user.Status = Status.CustomStatus;
                    user.Customstatus = CustomStatus.customStatus;
                    break;

                default:
                    return BadRequest("Not a Vaild Status");
            }
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
            
            return Ok("Status Changed");
        }
        [HttpPut]
        [Route("ToggleStarChat")]
        public IActionResult ToggleStarChat([FromBody] ChatIdDTO dto)
        {
            User user = this.GetAuthenticatedUser();

            IQueryable<int> userChatsIds = _unitOfWork.Users.GetChatsIdsByUserId(user.Id);

            UserChats userChat = userChatsIds.Any(uci => uci == dto.ChatID) ? _unitOfWork.UserChats.GetUserChatByIds(user.Id, dto.ChatID) : null;

            if (userChat == null)
                return BadRequest("This chat either does not exist or the user is not allowed to view this chat");

            userChat.IsStarred = !userChat.IsStarred;

            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();

            return Ok(new { isStarred = userChat.IsStarred });
        }
        [HttpPut]
        [Route("ToggleMuteChat")]
        public IActionResult ToggleMuteChat([FromBody] ChatIdDTO chat)
        {
            User user = this.GetAuthenticatedUser();

            IQueryable<int> userChatsIds = _unitOfWork.Users.GetChatsIdsByUserId(user.Id);

            UserChats userChat = userChatsIds.Any(uci => uci == chat.ChatID) ? _unitOfWork.UserChats.GetUserChatByIds(user.Id, chat.ChatID) : null;

            if (userChat == null)
                return BadRequest("This chat either does not exist or the user is not allowed to edit this chat");

            userChat.IsMuted = !userChat.IsMuted;

            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();

            return Ok(new { isMuted = userChat.IsMuted });
        }
        [HttpPut]
        [Route("MuteWorkspaceForAmountOfHours")]
        public IActionResult MuteWorkspaceForAmountOfHours([FromBody] MuteWorspaceForHoursDTO dto)
        {
            User user = this.GetAuthenticatedUser();

            IQueryable<int> userWorkspaceIds = _unitOfWork.Users.GetWorkspaceIdsByUserId(user.Id);

            var userWorkspaces = userWorkspaceIds.Any(uci => uci == dto.WorkspaceID) ? _unitOfWork.UserWorkspaces.GetUserWorkspaceByIds(user.Id, dto.WorkspaceID) : null;

            if (userWorkspaces == null)
                return BadRequest("This workspace either does not exist or the user is not allowed to edit this workspace");

            userWorkspaces.TimeToWhichNotificationsAreMuted = DateTime.Now.AddHours(dto.HoursToBeMuted);

            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();

            return Ok(new { Workpace = userWorkspaces.WorkspaceId, MutedUntil = userWorkspaces.TimeToWhichNotificationsAreMuted });
        }
        [HttpPost]
        [Route("SendInvitaionToUser")]
        public IActionResult SendInvitaionToUser([FromBody] EmailDTO mailTo)
        {
            User user = this.GetAuthenticatedUser();

           

                string pweda = "3adewelzew"; //(ConfigurationManager.AppSettings["password"]);
                string from = "zew.services.tgp@gmail.com"; //Replace this with your own correct Gmail Address
                                                            /*  string to = "abc@gef.com";*/ //Replace this with the Email Address to whom you want to send the mail
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.To.Add(mailTo.Email);
                mail.From = new MailAddress(from);
                mail.Subject = "This is a test mail";
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = "Test Mail.";
                mail.IsBodyHtml = false;

                mail.Priority = MailPriority.High;
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

                
                client.Credentials = new System.Net.NetworkCredential(from, pweda);
                client.Port = 587; 
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true; 

                try
                {
                    client.Send(mail);
                   return Ok("message Sent");
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex;
                    string errorMessage = string.Empty;
                    while (ex2 != null)
                    {
                        errorMessage += ex2.ToString();
                        ex2 = ex2.InnerException;
                    }
                return BadRequest(ex2);
            } 

               
        }

        [HttpGet]
        [Route("getprivatechat/{userId2}/{workspaceId}")]
        public IActionResult GetPrivateChat(int userId2, int workspaceId)
        {
            if (_unitOfWork.Users.GetByID(userId2) == null)
                return BadRequest("The user does not exist");

            if (_unitOfWork.Workspaces.GetByID(workspaceId) == null)
                return BadRequest("The workspace does not exist");

            User user1 = this.GetAuthenticatedUser();

            var commonChatIds = _unitOfWork.UserChats.GetCommonChats(user1.Id, userId2);

            Chat privateCommonChat;
            foreach (var commonChatId in commonChatIds)
            {
                privateCommonChat = _unitOfWork.Chats.GetChatIfPrivate(commonChatId, workspaceId);

                if (privateCommonChat != null)
                    return Ok(privateCommonChat);
            }

            UserChats userChat1 = new UserChats { UserId = user1.Id };
            UserChats userChat2 = new UserChats { UserId = userId2 };
            privateCommonChat = new Chat
            {
                CreatorID = user1.Id,
                DateCreated = DateTime.Now,
                IsPrivate = true,
                Name = "dm" + user1.Id + "," + userId2,
                WorkspaceId = workspaceId,
                UserChats = new List<UserChats> { userChat1, userChat2 }
            };

            _unitOfWork.Chats.Insert(privateCommonChat);
            _unitOfWork.Save();
            
            return Created("No Url at the moment", privateCommonChat);
        }
    }
}
