using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.BusinessLogic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkspaceController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public WorkspaceController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("{name}")]
        [Route("AddNewWorkspace")]
        public void AddNewWorkspaceByName(string name)
        {
            _unitOfWork.Workspaces.Add(new Workspace {WorkspaceName = name});
            _unitOfWork.Save();
        }
    }
}