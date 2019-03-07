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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using System.Net.Http.Headers;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zeww.BusinessLogic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspacesController : Controller
    {

        private IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;

        public WorkspacesController(IHostingEnvironment hostingEnvironment, IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._hostingEnvironment = hostingEnvironment;
        }

        // GET: /<controller>/ 
        [HttpGet]
        public IEnumerable<Workspace> Index()
        {
            return _unitOfWork.Workspaces.Get();
        }

        [HttpGet]
        [Route("GetWorkspaceName/{workspaceName}")]
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

        // POST api/CreateWorkspace/ 
        [HttpPost]
        [Route("CreateWorkspace")]
        public IActionResult CreateWorkspace([FromBody] Workspace newWorkspace)
        {
            if (!TryValidateModel(newWorkspace))
                return BadRequest(ModelState);

            var exist = _unitOfWork.Workspaces.GetWorkspaceByName(newWorkspace.WorkspaceName);

            if (exist != null)
                return BadRequest("Workspace name already exist");

            newWorkspace.DateOfCreation = DateTime.Now.ToString("MM/dd/yyyy");


            _unitOfWork.Workspaces.Insert(newWorkspace);
            _unitOfWork.Save();

            var addedWorkspace = _unitOfWork.Workspaces.GetWorkspaceByName(newWorkspace.WorkspaceName);
            var workspaceReference = newWorkspace.WorkspaceName + "/" + addedWorkspace.Id;

            var location = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(Request).Replace("CreateWorkspace", workspaceReference);

            addedWorkspace.URL = location;
            _unitOfWork.Workspaces.Update(addedWorkspace);
            _unitOfWork.Save();

            return Created(location, addedWorkspace);
        }


        [HttpPost]
        [Route("Upload/{id}")]
        public async Task<IActionResult> UploadWorkspaceImageAsync(int id)
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



    }



}
