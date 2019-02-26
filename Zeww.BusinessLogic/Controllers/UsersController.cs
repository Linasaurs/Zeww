using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zeww.BusinessLogic.DTOs;
using Zeww.DAL;
using Zeww.Models;
using Zeww.Repository;
using Zeww.BusinessLogic.ExtensionMethods;

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
        public string GetById(int Id) {
            return _unitOfWork.Users.GetByID(Id).Name;
        }

        // POST api/users
        [HttpPost]
        [Route("~/Post")]
        public void Post([FromBody] User user) {
            _unitOfWork.Users.Add(user);
            _unitOfWork.Save();
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
    }
}
