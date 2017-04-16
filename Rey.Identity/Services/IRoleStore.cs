using Rey.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Identity.Services {
    public interface IRoleStore<TRole> where TRole : class, IRole {
        void InsertOne(TRole role);
    }
}
