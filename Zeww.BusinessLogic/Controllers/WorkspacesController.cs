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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using System.Net.Http.Headers;
using Zeww.BusinessLogic.DTOs;
using Zeww.BusinessLogic.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using System.Net;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zeww.BusinessLogic.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspacesController : Controller {

        private IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;

        public WorkspacesController(IHostingEnvironment hostingEnvironment, IUnitOfWork unitOfWork) {
            this._unitOfWork = unitOfWork;
            this._hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IEnumerable<Workspace> Index() {
            //AddUserToWorkSpace(1, 1);
            return _unitOfWork.Workspaces.Get();
        }

        [HttpGet]
        [Route("GetWorkspaceName/{workspaceName}")]
        public IActionResult GetWorkspaceName(string workspaceName) {
            if (!string.IsNullOrWhiteSpace(workspaceName)) {
                var query = _unitOfWork.Workspaces.Get();
                if (query.Any(c => c.WorkspaceName.Contains(workspaceName)))
                    return Ok(workspaceName);
                else
                    return NotFound("There's no existing workspace with the specified name.");

            } else
                return BadRequest();
        }

        [HttpGet("GetWorkspaceByID/{id}")]
        public Workspace GetById(int Id) {
            return _unitOfWork.Workspaces.GetByID(Id);
        }

        [HttpPost]
        [Route("joinworkspace")]
        public IActionResult JoinWorkspace([FromQuery]int workspaceId, [FromQuery]string key)
        {
            User user = this.GetAuthenticatedUser();

            var workspace = _unitOfWork.Workspaces.GetByID(workspaceId);
            if (workspace == null)
                return BadRequest("The workspace does not exist");

            if (_unitOfWork.Workspaces.GetUsersIdInWorkspace(workspaceId).Contains(user.Id))
                return BadRequest("The user is already in the workspace");

            if(workspace.WorkspaceKey == key)
            {
                UserWorkspace userWorkspace = new UserWorkspace
                {
                    UserId = user.Id,
                    WorkspaceId = workspaceId,
                    UserRoleInWorkspace = UserRoleInWorkspace.Member
                };

                _unitOfWork.UserWorkspaces.Insert(userWorkspace);
                _unitOfWork.Save();

                return Ok("The user is successfully added to the workspace");
            }

            return BadRequest("The key you entered is not valid");
            
        }

        [HttpGet("GetUsersByWorkspaceId/{id}")]
        public ActionResult GetUsersByWorkspaceId(int Id) {
            if (Id < 1) {
                return BadRequest();
            }

            if (_unitOfWork.Workspaces.GetByID(Id) == null) {
                return NotFound();
            }
            var ListOfUsersIds = _unitOfWork.Workspaces.GetUsersIdInWorkspace(Id);
            var ListOfUsers = new List<User>();
            foreach (var userId in ListOfUsersIds)
            {
                ListOfUsers.Add(_unitOfWork.Users.GetByID(userId));
            }
            return Ok(ListOfUsers);
        }

        // POST api/NewWorkspace/workspacename
        [HttpPost]
        [Route("CreateWorkspace")]
        public IActionResult CreateWorkspace([FromBody] Workspace newWorkspace) {
            User user = this.GetAuthenticatedUser();

            if (!TryValidateModel(newWorkspace))
                return BadRequest(ModelState);

            var exist = _unitOfWork.Workspaces.GetWorkspaceByName(newWorkspace.WorkspaceName);

            if (exist != null)
                return BadRequest("Workspace name already exist");

            newWorkspace.DateOfCreation = DateTime.Now.ToString("MM/dd/yyyy");

            if (!TryValidateModel(newWorkspace))
                return BadRequest(ModelState);
            else {
                newWorkspace.CreatorID = user.Id;
                newWorkspace.WorkspaceKey = _unitOfWork.Workspaces.GenerateRandomString();
                newWorkspace.UserWorkspaces.Add(new UserWorkspace
                {
                    UserId = user.Id
                });
                _unitOfWork.Workspaces.Insert(newWorkspace);

            }
            _unitOfWork.Save();

            var addedWorkspace = _unitOfWork.Workspaces.GetWorkspaceByName(newWorkspace.WorkspaceName);
            var location = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(Request).Replace("CreateWorkspace", "" + addedWorkspace.Id);

            addedWorkspace.URL = location;
            _unitOfWork.Workspaces.Update(addedWorkspace);
            _unitOfWork.Save();

            return Created(location, addedWorkspace);


        }

        [HttpPut]
        [Route("WorkspaceDoNotDisturbPeriod/{workspaceId}")]
        public IActionResult WorkspaceDoNotDisturbPeriod([FromBody] DoNotDisturbDTO dto, int? workspaceId) {
            User user = this.GetAuthenticatedUser();

            

            var WorkspaceDoNotDisturbHours = _unitOfWork.Workspaces.GetByID(workspaceId);

            var from = dto.DoNotDisturbFrom;
            var to = dto.DoNotDisturbTo;

            if (WorkspaceDoNotDisturbHours == null) {
                return NotFound("this workspace id doesn't exist");
            }

            if (ModelState.IsValid) {
                if (WorkspaceDoNotDisturbHours.CreatorID == user.Id) {
                    WorkspaceDoNotDisturbHours.DailyDoNotDisturbFrom = from;
                    WorkspaceDoNotDisturbHours.DailyDoNotDisturbTo = to;

                    _unitOfWork.Save();

                    return NoContent();
                } else {
                    return BadRequest("not the correct user Id");
                }
            } else {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("EditWorkspaceName/{Id}")]
        public IActionResult EditWorkspaceName(int Id, [FromBody] Workspace workspace)
        {
            var workspaceNameToEdit = _unitOfWork.Workspaces.GetByID(Id);
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
        [Route("EditWorkspaceURL/{Id}")]
        public IActionResult EditWorkspaceURL(int Id, [FromBody] WorkspaceUrlDTO workspaceDTO)
        {
            var workspaceURLToEdit = _unitOfWork.Workspaces.GetByID(Id);
            if (workspaceURLToEdit != null)
            {
                workspaceURLToEdit.URL = workspaceDTO.URL;
                _unitOfWork.Workspaces.Update(workspaceURLToEdit);
                _unitOfWork.Save();
                return Ok();
            } else {
                return BadRequest();
            }
        }

        //}

        //Delete Workspace
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteWorkspace(int id) {
            if (id <= 0)
                return BadRequest("ID must be greater than zero");

            var workspace = _unitOfWork.Workspaces.GetByID(id);

            if (workspace == null)
                return NotFound();

            _unitOfWork.Workspaces.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }


        [HttpPost]
        [Route("AddUserToWorkspace")]
        public void AddUserToWorkspace(int userId, int workspaceId) {
            var workspace = _unitOfWork.Workspaces.GetByID(workspaceId);
            var user = _unitOfWork.Users.GetByID(userId);
            var uw = new UserWorkspace {
                UserId = userId,
                WorkspaceId = workspaceId,
                /* 
                 * The -1 subtracts 1 hour from the current time,
                 * this makes notifications enabled by default.
                 */
                TimeToWhichNotificationsAreMuted = DateTime.Now.AddHours(-1),
                UserRoleInWorkspace = UserRoleInWorkspace.Member
            };
            user.UserWorkspaces.Add(uw);
            workspace.UserWorkspaces.Add(uw);
            _unitOfWork.UserWorkspaces.Insert(uw);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Workspaces.Update(workspace);
            _unitOfWork.Save();

        }


        [HttpPut]
        [Route("ChangeWorkspaceMemberRole/{workspaceId}")]
        public IActionResult ChangeWorkspaceMemberRole(int workspaceId, [FromBody] WorkspaceRoleDTO dto) {
            var user = this.GetAuthenticatedUser();
            var workspace = _unitOfWork.Workspaces.GetByID(workspaceId);
            if (workspace == null) {
                return BadRequest("This workspace does not exist");
            }
            var userToBeChanged = _unitOfWork.Users.GetByID(dto.UserToBeChangedId);
            if (userToBeChanged == null) {
                return BadRequest("The user to be changed does not exist");
            }
            var userWorkspace = _unitOfWork.UserWorkspaces.GetUserWorkspaceByIds(user.Id, workspaceId);
            if (userWorkspace == null) {
                return BadRequest("The authenticated user is not a member of this workspace or does not have admin privileges to it.");
            }
            if (userWorkspace.UserRoleInWorkspace != UserRoleInWorkspace.Owner && userWorkspace.UserRoleInWorkspace != UserRoleInWorkspace.Admin) {
                return BadRequest("The authenticated user is not a member of this workspace or does not have admin privileges to it.");
            }
            userWorkspace = _unitOfWork.UserWorkspaces.GetUserWorkspaceByIds(dto.UserToBeChangedId, workspaceId);
            if (userWorkspace == null) {
                return BadRequest("The user to be changed does not exist in this workspace");
            }
            userWorkspace.UserRoleInWorkspace = dto.UserRoleInWorkspace;
            _unitOfWork.UserWorkspaces.Update(userWorkspace);
            _unitOfWork.Save();

            return Ok(userWorkspace);
        }


        [HttpPost]
        [Route("Upload/{id}")]
        public async Task<IActionResult> UploadWorkspaceImageAsync(int id)
        {
            try
            {
                long size = 0;

                Workspace workspace = _unitOfWork.Workspaces.GetByID(id);

                var originalImageName = "";
                string imageId = Guid.NewGuid().ToString().Replace("-", "");

                var path = Path.Combine(_hostingEnvironment.WebRootPath, "Images", imageId);

                var files = Request.Form.Files;
                var file = files.FirstOrDefault();

                originalImageName = Path.GetFileName(file.FileName);

                var fileExtension = Path.GetExtension(file.FileName);
                var fullPath = $"{path}{fileExtension}";

                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                var returnedPath = Request.Scheme + "://" + Request.Host + "/Images/" + imageId + fileExtension;

                workspace.WorkspaceImageId = returnedPath;
                workspace.WorkspaceImageName = originalImageName;

                _unitOfWork.Workspaces.Update(workspace);
                _unitOfWork.Save();

                string message = $"{files.Count} {size} bytes uploaded successfully!";
                return Json(returnedPath);
            }
            catch (Exception ex)
            {
                return Ok(ex);
                throw;
            }
        }

        [HttpPut]
        [Route("ToggleDisplayEmailsInMembersProfile")]
        public IActionResult ToggleDisplayEmailsInMembersProfile([FromBody] WorkspaceIdDto dto) {
            User user = this.GetAuthenticatedUser();
            Workspace workspace = _unitOfWork.Workspaces.GetByID(dto.WorkspaceId);
            if (workspace == null) {
                return NotFound();
            }
            if (workspace.CreatorID != user.Id) {
                return Unauthorized("You are not the Workspace Admin");
            }
            if (workspace.CreatorID == user.Id) {
                workspace.IsEmailVisible = !workspace.IsEmailVisible;
            }
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
            return Ok(new { isVisible = workspace.IsEmailVisible });
        }

        [HttpGet]
        [Route("OmniSearch")]
        public IActionResult OmniSearch(string searchQuery, int workspaceId)
        {
            //It does two web requests with that string
            //First the search for all channels with that username
            var returnedChannels = _unitOfWork.Workspaces.SearchForChannelInWorkspace(searchQuery, workspaceId);
            //Search for all users in workspace
            var listOfUserIdsInWorkspace = _unitOfWork.Workspaces.GetUsersIdInWorkspace(workspaceId);
            var returnedUsers = new List<User>();
            foreach (int userId in listOfUserIdsInWorkspace)
            {
                returnedUsers.Add(_unitOfWork.Users.GetByID(userId));
            }
            returnedUsers = returnedUsers.Where(u => u.Name.Contains(searchQuery)).ToList<User>();
            var returnedObject = new { returnedChannels = returnedChannels, returnedUsers = returnedUsers };
            return Ok(returnedObject);

        }
    }
}







