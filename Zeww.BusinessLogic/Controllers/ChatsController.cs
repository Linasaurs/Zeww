using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.BusinessLogic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : Controller {
        private IUnitOfWork _unitOfWork;

        public ChatsController(IUnitOfWork unitOfWork) {
            this._unitOfWork = unitOfWork;
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

        //This is a test code for Wael , use if needed else ignore it (Creates a Chat)
        [HttpPost("PostChat")]
        public IActionResult PostChat([FromBody]Chat chat)
        {
            if (chat != null)
            {
                _unitOfWork.Chats.Insert(chat);
                _unitOfWork.Save();
                var insertedChat = _unitOfWork.Chats.Get().Where(element=> element.Name == chat.Name);
                return Ok(insertedChat);
            }
            return BadRequest();
        }



        [HttpGet("GetFiles/{chatName}")]
        public IActionResult GetFiles(string chatName,[FromQuery]string SenderName, [FromQuery]string topic)
        {
            var returnedFileList = _unitOfWork.Files.GetFiles(chatName, SenderName, topic);
            if (returnedFileList != null)
                return Ok(returnedFileList);
            return NotFound();
        }

        [HttpPost("PostFile")]
        public IActionResult PostFile([FromBody]File file)
        {
            if(file != null)
            {
                _unitOfWork.Files.Add(file);
                _unitOfWork.Save();
                var insertedFile = _unitOfWork.Files.GetByID(_unitOfWork.Files.Get().Count());
                return Ok(insertedFile);
            }
            return BadRequest();
        }


        [HttpPut]
        [Route("EditChannelPurpose/{channelId}")]
        public IActionResult EditChannelPurpose(Chat chat, int channelId) {
            //Ziad is still working on that method

            return Ok();
        }

        [HttpGet]
        [Route("SearchByChannelName/{channelName}")]
        public IActionResult SearchByChannelName(String channelName) {
            //This code is written by Hanna and replicated here
            if (!string.IsNullOrWhiteSpace(channelName)) {
                var query = _unitOfWork.Chats.Get();
                if (query.Any(c => c.Name.Contains(channelName)))
                    return Ok(channelName);
                else
                    return NotFound("Could ot find a channel with that name, Sorry!");

            } else
                return BadRequest();
        }

        [HttpPost("AddUserToChannel")]
        public IActionResult AddUserToChannel([FromBody] UserChats UserChat)
        {
            User user = _unitOfWork.Users.GetByID(UserChat.UserId);
            Chat chat = _unitOfWork.Chats.GetByID(UserChat.ChatId);

            if (user != null && chat != null)
            {
                _unitOfWork.UserChats.Insert(UserChat);
                _unitOfWork.Save();
                //send message to channel ----- call taher's function
                return Ok(UserChat);
            }
            else return BadRequest();
        }

    }
}
