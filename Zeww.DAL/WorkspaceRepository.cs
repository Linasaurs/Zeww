using System;
using System.Collections.Generic;
using System.Text;
using Zeww.Models;
using Zeww.Repository; 

namespace Zeww.DAL {
    public class WorkspaceRepository : GenericRepository<Workspace>, IWorkspaceRepository {
        //This sets the context of the child class to the context of the super class
        public WorkspaceRepository(ZewwDbContext context) : base(context) { }
        
        //Your methods go here 
        public void Add(Workspace newWorkspace)
        {
            dbSet.Add(newWorkspace);
        }
    }
}
