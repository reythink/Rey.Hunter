using Rey.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rey.Identity.Services {
    public interface IUserStore<TUser> where TUser : class, IUser {
        TUser GetUserByClaims(IEnumerable<Claim> claims);
        void InsertOne(TUser user);
    }
}
