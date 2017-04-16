using Rey.Identity.Models;
using Microsoft.AspNetCore.Http.Authentication;
using System;
using System.Threading.Tasks;

namespace Rey.Identity.Services {
    public interface ILoginManager<TUser>
        where TUser : class, IUser {
        Task LoginAsync(TUser user);
        Task LoginAsync(TUser user, AuthenticationProperties properties);

        Task LogoutAsync();
        Task LogoutAsync(AuthenticationProperties properties);

        TUser SetPassword(TUser user, string password);
        bool VerifyPassword(TUser user, string password);

        TUser Register(TUser user);
    }

    public interface ILoginManager<TUser, TRole> : ILoginManager<TUser>
        where TUser : class, IUser
        where TRole : class, IRole {
        TUser Register(TRole role, Func<TRole, TUser> configUser);
    }

    public interface ILoginManager<TUser, TRole, TAccount> : ILoginManager<TUser, TRole>
        where TUser : class, IUser
        where TRole : class, IRole
        where TAccount : class, IAccount {
        TUser Register(TAccount account, Func<TAccount, TRole> configRole, Func<TAccount, TRole, TUser> configUser);
    }
}
