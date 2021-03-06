﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeww.Models;

namespace Zeww.Repository
{
    public interface IWorkspaceRepository : IGenericRepository<Workspace> {

        //Your method headers go here
        Workspace GetWorkspaceByName(string name);
        IQueryable<int> GetUsersIdInWorkspace(int id);
        IQueryable <Chat> GetAllChannelsInAworkspace(int workspaceId);
        IQueryable<Chat> SearchForChannelInWorkspace(string queryString, int workspaceId);
        string GenerateRandomString();
    }
}
