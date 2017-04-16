using Rey.Authority.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Authority.Models {
    public static class MemoryAuthStoreOptionsExtensions {
        public static MemoryAuthStoreOptions Add(this MemoryAuthStoreOptions options, IAuthTarget target, IEnumerable<IAuthActivity> activities) {
            options.Items.Add(new AuthItem(target, activities));
            return options;
        }

        public static MemoryAuthStoreOptions Add(this MemoryAuthStoreOptions options, IAuthTarget target, params IAuthActivity[] activities) {
            options.Items.Add(new AuthItem(target, activities));
            return options;
        }
    }
}
