using Rey.Identity.Configuration;
using Rey.Identity.Models;
using Rey.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection {
    public static class IdentityOptionsExtensions {
        public static IdentityOptions AddMongoUserStore<TUser>(this IdentityOptions identity)
            where TUser : class, IUser {
            identity.Services.AddSingleton<IUserStore<TUser>, MongoUserStore<TUser>>();
            return identity;
        }
    }
}
