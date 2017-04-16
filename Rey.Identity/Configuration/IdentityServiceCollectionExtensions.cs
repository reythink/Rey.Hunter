using Rey.Identity.Configuration;
using Rey.Identity.Models;
using Rey.Identity.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection {
    public static class IdentityServiceCollectionExtensions {
        public static IServiceCollection AddReyIdentity<TUser, TRole, TAccount>(this IServiceCollection services, IdentityOptions<TUser, TRole, TAccount> options)
            where TUser : class, IUser
            where TRole : class, IRole
            where TAccount : class, IAccount {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IdentityOptions>(options);

            services.AddSingleton<ILoginManager<TUser, TRole, TAccount>, LoginManager<TUser, TRole, TAccount>>();
            services.AddSingleton<ILoginManager<TUser, TRole>, LoginManager<TUser, TRole, TAccount>>();
            services.AddSingleton<ILoginManager<TUser>, LoginManager<TUser, TRole, TAccount>>();

            services.AddSingleton<ILoginContext<TUser>, LoginContext<TUser>>();

            return services;
        }

        public static IServiceCollection AddReyIdentity<TUser, TRole, TAccount>(this IServiceCollection services, Action<IdentityOptions<TUser, TRole, TAccount>> config = null)
            where TUser : class, IUser
            where TRole : class, IRole
            where TAccount : class, IAccount {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var options = new IdentityOptions<TUser, TRole, TAccount>(services);
            config?.Invoke(options);
            return AddReyIdentity<TUser, TRole, TAccount>(services, options);
        }

        //public static IServiceCollection AddReyIdentity<TUser>(this IServiceCollection services, IdentityOptions options)
        //    where TUser : class, IUser {
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));

        //    if (options == null)
        //        throw new ArgumentNullException(nameof(options));

        //    services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //    services.AddSingleton(options);
        //    services.AddSingleton<ILoginManager<TUser>, LoginManager<TUser>>();
        //    services.AddSingleton<ILoginContext<TUser>, LoginContext<TUser>>();
        //    return services;
        //}

        //public static IServiceCollection AddReyIdentity<TUser>(this IServiceCollection services, Action<IdentityOptions> config = null)
        //    where TUser : class, IUser {
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));

        //    var options = new IdentityOptions(services);
        //    config?.Invoke(options);
        //    return AddReyIdentity<TUser>(services, options);
        //}
    }
}
