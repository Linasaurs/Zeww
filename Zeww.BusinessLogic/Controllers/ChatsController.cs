using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //This is code for Ziad, please do not touch this method
        [HttpPost]
        [Route("CreateNewChannel")]
        public IActionResult CreateNewChannel(Chat chat) {
            _unitOfWork.Chats.Insert(chat);
            _unitOfWork.Save();
            return Ok();
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
        public IActionResult GetFiles(string chatName,[FromQuery]string SenderName)
        {
            var returnedFileList = _unitOfWork.Files.GetFilesBySenderName(SenderName , chatName);
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
