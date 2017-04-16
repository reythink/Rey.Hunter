using Rey.Identity.Models;
using Rey.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Rey.Identity.Configuration {
    public abstract class IdentityOptions {
        public IServiceCollection Services { get; }
        public string Scheme { get; set; }

        public IdentityOptions(IServiceCollection services) {
            this.Services = services;
        }
    }

    public class IdentityOptions<TUser, TRole, TAccount> : IdentityOptions
        where TUser : class, IUser
        where TRole : class, IRole
        where TAccount : class, IAccount {
        public IdentityOptions(IServiceCollection services)
            : base(services) {
        }

        public void AddUserStore<TUserStore>()
            where TUserStore : class, IUserStore<TUser> {
            this.Services.AddSingleton<IUserStore<TUser>, TUserStore>();
        }

        public void AddRoleStore<TRoleStore>()
            where TRoleStore : class, IRoleStore<TRole> {
            this.Services.AddSingleton<IRoleStore<TRole>, TRoleStore>();
        }

        public void AddAccountStore<TAccountStore>()
            where TAccountStore : class, IAccountStore<TAccount> {
            this.Services.AddSingleton<IAccountStore<TAccount>, TAccountStore>();
        }
    }
}
