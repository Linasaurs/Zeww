using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeww.Models;

namespace Zeww.Repository
{
    public interface IWorkspaceRepository : IGenericRepository<Workspace> {

        //Your method headers go here
        IQueryable<int> GetUsersIdInWorkspace(int id);
    }
}
