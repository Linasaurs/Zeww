using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zeww.DAL;
using Zeww.Models;
using Zeww.Repository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zeww.BusinessLogic.Controllers
{
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
        public string GetById(int Id) {
            return _unitOfWork.Users.GetByID(Id).Name;
        }

        // POST api/users
        [HttpPost]
        //[Route("~/Post")]
        public void Post([FromBody] User user) {
            _unitOfWork.Users.Add(user);
            _unitOfWork.Save();
        }


    }
}
