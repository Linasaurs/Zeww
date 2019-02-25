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
    public class ChatsController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public ChatsController(IUnitOfWork unitOfWork) {
            this._unitOfWork = unitOfWork;
        }

        // GET: /<controller>/
        public string Index() {
            var file = new File { source = "google.com", Size = 20, Extension = "rr" };
            _unitOfWork.Files.Insert(file);

            return _unitOfWork.Files.Get().Count().ToString();
        }

        //This is code for Ziad, please do not touch this method
        [Route("~/CreateNewChannel")]
        public string CreateNewChannel(Chat chat) {

            return "passable";
        }

        [HttpGet("{channelName}")]
        public IActionResult GetChannelFiles(string channelName, string SenderName)
        {
            var returnedFileList = _unitOfWork.Files.GetFilesBySenderName(SenderName , channelName);
            if (returnedFileList != null)
            {
                return Ok(returnedFileList);
            }

            return NotFound();

        }
    }
}
