using Rey.Authority.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Authority.Configuration {
    public class MemoryAuthStoreOptions {
        public List<IAuthItem> Items { get; } = new List<IAuthItem>();
    }
}
