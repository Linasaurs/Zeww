﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zeww.DAL;
using Zeww.Models;
using Zeww.Repository;
using System.Web.Http;
using System.Net.Http;
using Newtonsoft.Json;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zeww.BusinessLogic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspacesController : Controller
    {

        private IUnitOfWork _unitOfWork;


        public WorkspacesController(IUnitOfWork unitOfWork)
        {
           this._unitOfWork = unitOfWork;
        }

        // GET: /<controller>/
        public IEnumerable<Workspace> Index()
        {
            AddUserToWorkSpace(1, 1);
            return _unitOfWork.Workspaces.Get();
        }

        [HttpGet]
        [Route("name/{workspaceName}")]
        public IActionResult GetWorkspaceName(string workspaceName)
        {
            if (!string.IsNullOrWhiteSpace(workspaceName))
            {
                var query = _unitOfWork.Workspaces.Get();
                if (query.Any(c => c.WorkspaceName.Contains(workspaceName)))
                    return Ok(workspaceName);
                else
                    return NotFound("There's no existing workspace with the specified name.");

            }
            else
                return BadRequest();

        }
        [HttpGet("{id}")]
        public string GetById(int Id)
        {
            return _unitOfWork.Workspaces.GetByID(Id).WorkspaceName;
        }

        [HttpGet("{id}/users")]
        public ActionResult GetUsersByWorkspaceId(int Id)
        {
            if (Id < 1)
            {
                return BadRequest();
            }


            if (_unitOfWork.Workspaces.GetByID(Id) == null)
            {
                return NotFound();
            }


            return Ok(_unitOfWork.UserWorkspaces.GetWorkspacesByUserId(Id));


        }

        // POST api/NewWorkspace/workspacename
        [HttpPost]
        [Route("NewWorkspace/{name}")]
        public IActionResult AddNewWorkspaceByName(string name, [FromBody] [Bind] Optionals Optionals)
        {
            Workspace newWorkspace;

            if (Optionals != null)
                newWorkspace = new Workspace(Optionals) { WorkspaceName = name, DateOfCreation = DateTime.Now.ToString("MM/dd/yyyy") };
            else
                newWorkspace = new Workspace { WorkspaceName = name, DateOfCreation = DateTime.Now.ToString("MM/dd/yyyy"), CompanyName = Optionals.CompanyName };

            if (!TryValidateModel(newWorkspace))
                return BadRequest(ModelState);
            else
                _unitOfWork.Workspaces.Insert(newWorkspace);
            _unitOfWork.Save();

            return Ok(newWorkspace);
        }

        [HttpPost]
        public void AddUserToWorkSpace(int userId, int workspaceId)
        {
            var workspace = _unitOfWork.Workspaces.GetByID(workspaceId);
            var user = _unitOfWork.Users.GetByID(userId);
            var uw = new UserWorkspace {UserId=userId,User=user,WorkspaceId=workspaceId,
            Workspace=workspace};
            _unitOfWork.UserWorkspaces.Insert(uw);
            user.UserWorkspaces.Add(uw);
            //workspace.UserWorkspaces.Add(uw);
            _unitOfWork.Save();

        }
    }
}
