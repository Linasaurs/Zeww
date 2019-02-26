using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zeww.DAL;
using Zeww.Models;
using Zeww.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zeww.BusinessLogic.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public MessagesController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        // GET: api/messages
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Message Retrieved");
        }

        // GET api/messages/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int Id)
        {
            return Ok(_unitOfWork.Messages.GetByID(Id).MessageContent);
        }

        [HttpPost]
        //[Route("~/Post")]
        public IActionResult Post([FromBody] Message message)
        {
            _unitOfWork.Messages.Add(message);
            _unitOfWork.Save();
            return Ok("Message Added Succesfully");

        }

    }
}
