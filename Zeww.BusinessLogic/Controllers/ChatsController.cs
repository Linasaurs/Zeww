﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeww.BusinessLogic.ExtensionMethods;
using Zeww.Models;
using Zeww.Repository;
using File = Zeww.Models.File;

namespace Zeww.BusinessLogic.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : Controller {
        private IUnitOfWork _unitOfWork;
        private IHostingEnvironment _hostingEnvironment; 

        public ChatsController(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment)
        {
            this._unitOfWork = unitOfWork;
            this._hostingEnvironment = hostingEnvironment;
        }

        // GET: /<controller>/
        [HttpGet]
        public string Index() {
            return "Hello";
        }


        [HttpPost]
        [Route("CreateNewChannel")]
        public IActionResult CreateNewChannel(Chat chat) {
            _unitOfWork.Chats.Insert(chat);
            _unitOfWork.Save();
            var returnedChat = _unitOfWork.Chats.Get(ch => ch.Name == chat.Name && ch.WorkspaceId == chat.WorkspaceId);
            return Ok(returnedChat);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetChannelDetails/{chatID}")]
        public IActionResult GetChannelDetails(int? chatID) {
            var chatDetails = _unitOfWork.Chats.GetByID(chatID);
            var numberOfUsers = _unitOfWork.UserChats.GetNumberOfUsersInChat(chatID);
            var workspaceUrl = _unitOfWork.Workspaces.GetByID(chatDetails.WorkspaceId);
            var url = workspaceUrl.URL;
            if (chatDetails==null)
            {
                return NotFound();
            }
           else if (chatDetails.Id.Equals(chatID))
            {
                var channelDetails = new { chatDetails, numberOfUsers, url };
                return Ok(channelDetails);
            }
            return NotFound();

        }


        //This is a test code for Wael , use if needed else ignore it (Creates a Chat)
        [HttpPost("PostChat")]
        public IActionResult PostChat([FromBody]Chat chat) {
            if (chat != null) {
                _unitOfWork.Chats.Insert(chat);
                _unitOfWork.Save();
                var insertedChat = _unitOfWork.Chats.Get().Where(element => element.Name == chat.Name);
                return Ok(insertedChat);
            }
            return BadRequest();
        }



        [HttpGet("GetFiles/{chatName}")]
        public IActionResult GetFiles(string chatName, [FromQuery]string SenderName, [FromQuery]string topic) {
            var returnedFileList = _unitOfWork.Files.GetFiles(chatName, SenderName, topic);
            if (returnedFileList != null)
                return Ok(returnedFileList);
            return NotFound();
        }

        [HttpPost("PostFile")]
        public IActionResult PostFile([FromBody]File file) {
            if (file != null) {
                _unitOfWork.Files.Add(file);
                _unitOfWork.Save();
                var insertedFile = _unitOfWork.Files.GetByID(_unitOfWork.Files.Get().Count());
                return Ok(insertedFile);
            }
            return BadRequest();
        }


        [HttpPut]
        [Route("EditChannelTopic/{channelId}")]
        public IActionResult EditChannelTopic(int channelId, [FromQuery]string topic) {
            if (!String.IsNullOrEmpty(topic) && channelId != 0) {
                var success = _unitOfWork.Chats.EditChatTopic(channelId, topic);
                _unitOfWork.Save();
                if (success) {
                    return Ok(_unitOfWork.Chats.GetByID(channelId));
                }
                return NotFound();
            }
            return BadRequest();
        }

       
        public IQueryable<Chat> GetAllChannelsInWorkspace(int workspaceId) {
            var listOfChannelsInAWorkspace = _unitOfWork.Workspaces.GetAllChannelsInAworkspace(workspaceId);
            if (listOfChannelsInAWorkspace == null)
                return null;
            return listOfChannelsInAWorkspace;
        }

        [HttpGet]
        [Route("GetAllChannelsInsideWorkspace")]
        public IActionResult GetAllChannelsInsideWorkspace(int workspaceId) {
            var listOfChannelsInAWorkspace = _unitOfWork.Workspaces.GetAllChannelsInAworkspace(workspaceId);
            if (listOfChannelsInAWorkspace == null)
                return BadRequest("No workspace with this id");
            return Ok(listOfChannelsInAWorkspace);
        }


        [HttpGet]
        [Route("SearchByChannelName/{channelName}")]
        public IActionResult SearchByChannelName(String channelName, int workspaceId) {

            if (!string.IsNullOrWhiteSpace(channelName)) {
                var queryOfChannels = _unitOfWork.Workspaces.GetAllChannelsInAworkspace(workspaceId);
                if (queryOfChannels.Any(c => c.Name.ToLower().Contains(channelName)))
                    return Ok(queryOfChannels);
                else
                    return NotFound("Could not find a channel with that name, Sorry!");
            } else
                return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("AddUserToChannel/{channelId}")]
        public IActionResult AddUserToChannel(int channelId, [FromBody] string UserName)
        {
            UserChats userChat = new UserChats();
            User user = _unitOfWork.Users.GetUserByUserName(UserName);
            Chat chat = _unitOfWork.Chats.GetByID(channelId);

            if (user != null && chat != null)
            {
                userChat.ChatId = channelId;
                userChat.UserId = user.Id;
                _unitOfWork.UserChats.Insert(userChat);
                _unitOfWork.Save();
                //send message to channel ----- call taher's function
                return Ok(userChat);
            }
            else return BadRequest();
        }


        [HttpPut("EditChannelName/{channelId}")]
        public IActionResult EditChannelName(int channelId, [FromQuery]string newName)
        {
            User user = this.GetAuthenticatedUser();
            Chat chat = _unitOfWork.Chats.GetByID(channelId);
            if (chat != null)
            {
                if (chat.CreatorID == user.Id)
                {
                    _unitOfWork.Chats.EditChannelName(channelId, newName);
                    _unitOfWork.Save();
                    return Ok("Channel Name has been changed to " + newName);
                }
                return Unauthorized();
            }
            return NotFound();
        }
        [HttpGet("MessagesDate")]
        public IActionResult viewMessagesDate()
        {
            return Ok();
        }



        [HttpPost]
        [Route("UploadFile/{id}")]
        public async Task<IActionResult> UploadFile(int id)
        {
            long size = 0;

            User uploadedUser = this.GetAuthenticatedUser(); 

            var originalImageName = "";
            string fileId = Guid.NewGuid().ToString().Replace("-", "");

            var path = Path.Combine(_hostingEnvironment.WebRootPath, "Files", fileId);

            var files = Request.Form.Files;
            var file = files.FirstOrDefault();

            originalImageName = Path.GetFileName(file.FileName);

            var fileExtension = Path.GetExtension(file.FileName);
            var fullPath = $"{path}{fileExtension}";

            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            var returnedPath = Request.Scheme + "://" + Request.Host + "/Files/" + fileId + fileExtension;

            File uploadedFile = new File { Source = returnedPath, Name = originalImageName, Extension = fileExtension, UserId = uploadedUser.Id, ChatId = id };
            _unitOfWork.Files.Add(uploadedFile);
            _unitOfWork.Save();

            string message = $"{files.Count} {size} bytes uploaded successfully!";

            return Json(returnedPath);
        }

    }
}
