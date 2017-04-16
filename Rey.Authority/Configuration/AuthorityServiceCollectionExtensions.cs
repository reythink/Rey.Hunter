using Rey.Authority.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection {
    public static class AuthorityServiceCollectionExtensions {
        public static IServiceCollection AddReyAuthority(this IServiceCollection services, AuthorityOptions options) {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            services.AddSingleton(options);
            return services;
        }

        public static IServiceCollection AddReyAuthority(this IServiceCollection services, Action<AuthorityOptions> config = null) {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var options = new AuthorityOptions(services);
            config?.Invoke(options);
            return AddReyAuthority(services, options);
        }
    }
}
