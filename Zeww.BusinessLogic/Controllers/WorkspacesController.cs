using System;
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
using Zeww.BusinessLogic.DTOs;
using Zeww.BusinessLogic.ExtensionMethods;


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
        [HttpGet]
        public IEnumerable<Workspace> Index()
        {
            return _unitOfWork.Workspaces.Get();
        }


        [HttpGet("{id}")]
        public string GetById(int Id)
        {
            return _unitOfWork.Workspaces.GetByID(Id).WorkspaceName;
        }

        // POST api/CreateWorkspace/ 
        [HttpPost]
        [Route("CreateWorkspace")]
        public IActionResult CreateWorkspace([FromBody] Workspace newWorkspace)
        {
            var location = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(Request).Replace("CreateWorkspace", newWorkspace.WorkspaceName); ;

            newWorkspace.DateOfCreation = DateTime.Now.ToString("MM/dd/yyyy");
            newWorkspace.URL = location;


            if (!TryValidateModel(newWorkspace))
                return BadRequest(ModelState);
            else
                _unitOfWork.Workspaces.Insert(newWorkspace);
            _unitOfWork.Save();

            return Created(location, newWorkspace);
        }

        [HttpPut("{workspaceId}")]
        [Route("WorkspaceDoNotDisturbPeriod/{workspaceId}")]
        public IActionResult WorkspaceDoNotDisturbPeriod([FromBody] DoNotDisturbDTO dto, int? workspaceId)
        {
            User user = this.GetAuthenticatedUser();

            var WorkspaceDoNotDisturbHours = _unitOfWork.Workspaces.GetByID(workspaceId);

            var from = dto.DoNotDisturbFrom;
            var to = dto.DoNotDisturbTo;

            if (WorkspaceDoNotDisturbHours == null)
            {
                return NotFound("this workspace id doesn't exist");
            }

            if (ModelState.IsValid)
            {
                if (WorkspaceDoNotDisturbHours.CreatorID == user.Id)
                {
                    WorkspaceDoNotDisturbHours.DailyDoNotDisturbFrom = from;
                    WorkspaceDoNotDisturbHours.DailyDoNotDisturbTo = to;

                    _unitOfWork.Save();

                    return NoContent();
                }
                else
                {
                    return BadRequest("not the correct user Id");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

            
        }

        [HttpPut]
        [Route("EditWorkspaceName")]
        public IActionResult EditWorkspaceName([FromBody] Workspace workspace)
        {
            var workspaceNameToEdit = _unitOfWork.Workspaces.GetByID(workspace.Id);
            if (workspaceNameToEdit == null)
            {
                return BadRequest();
            }
            workspaceNameToEdit.WorkspaceName = workspace.WorkspaceName;
            _unitOfWork.Workspaces.Update(workspaceNameToEdit);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpPut]
        [Route("EditWorkspaceURL/{workspace.Id}")]
        public IActionResult EditWorkspaceURL([FromBody] Workspace workspace)
        {
            var workspaceURLToEdit = _unitOfWork.Workspaces.Get().Where(w => w.Id == workspace.Id).FirstOrDefault();
            if (workspaceURLToEdit != null && workspaceURLToEdit.WorkspaceName== workspace.WorkspaceName)
            {

                workspaceURLToEdit.URL = workspace.URL;
                _unitOfWork.Workspaces.Update(workspaceURLToEdit);
                _unitOfWork.Save();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
            
        }
    }
}
