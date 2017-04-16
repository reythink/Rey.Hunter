using Rey.Authority.Configuration;
using Rey.Authority.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection {
    public static class AuthorityOptionsExtensions {
        public static AuthorityOptions AddMemoryAuthStore(this AuthorityOptions authority, MemoryAuthStoreOptions options) {
            if (authority == null)
                throw new ArgumentNullException(nameof(authority));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            authority.Services.AddSingleton(options);
            authority.Services.AddSingleton<IAuthStore, MemoryAuthStore>();
            return authority;
        }

        public static AuthorityOptions AddMemoryAuthStore(this AuthorityOptions authority, Action<MemoryAuthStoreOptions> config = null) {
            if (authority == null)
                throw new ArgumentNullException(nameof(authority));

            var options = new MemoryAuthStoreOptions();
            config?.Invoke(options);
            return AddMemoryAuthStore(authority, options);
        }
    }
}
