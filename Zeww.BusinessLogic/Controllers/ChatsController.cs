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
            return Ok();
        }

        [HttpPut]
        [Route("EditChannelPurpose")]
        public IActionResult EditChannelPurpose(Chat chat, String newName) {
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
                    return NotFound("There's no existing workspace with the specified name.");

            } else
                return BadRequest();
        }

    }
}
