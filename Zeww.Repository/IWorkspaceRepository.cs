using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;

namespace Zeww.Repository
{
    public interface IWorkspaceRepository : IGenericRepository<Workspace> {

        //Your method headers go here
        void Add(Workspace newWorkspace);
    }
}
