using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zeww.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zeww.BusinessLogic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelsController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public ChannelsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet("{channelName}")]
        public IActionResult GetChannelFiles(string channelName ,string SenderName)
        {
            var returnedFileList = _unitOfWork.Files.GetFilesBySenderName(SenderName);
            if(returnedFileList != null)
            {
                return Ok(returnedFileList);
            }

            return NotFound();
            
        }

    }
}
