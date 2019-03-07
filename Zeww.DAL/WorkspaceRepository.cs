﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeww.Models;
using Zeww.Repository;

namespace Zeww.DAL
{
    public class WorkspaceRepository : GenericRepository<Workspace>, IWorkspaceRepository
    {
        //This sets the context of the child class to the context of the super class
        public WorkspaceRepository(ZewwDbContext context) : base(context) { }

        //Your methods go here  
        public Workspace GetWorkspaceByName(string name)
        {     
            IQueryable<Workspace> query = dbSet;
            return query.SingleOrDefault(ws => ws.WorkspaceName == name);
        }


        //Your methods go here 
        public IQueryable<int> GetUsersIdInWorkspace(int id)
        {
            IQueryable<Workspace> queryWorksapces = dbSet;
            var workspaces= queryWorksapces.Where(w=>w.Id==id).Include(w=>w.UserWorkspaces).Select(uw => uw.UserWorkspaces);
            return workspaces.SelectMany(uw=>uw.Select(u=>u.UserId));
        }

    }
}
