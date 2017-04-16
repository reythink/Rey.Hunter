using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rey.Authority.Models;
using Rey.Authority.Configuration;

namespace Rey.Authority.Services {
    public class MemoryAuthStore : IAuthStore {
        private MemoryAuthStoreOptions Options { get; }

        public MemoryAuthStore(MemoryAuthStoreOptions options) {
            this.Options = options;
        }

        public IEnumerable<IAuthItem> GetAuthItems() {
            return this.Options.Items;
        }
    }
}
