using Rey.Authority.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Authority.Services {
    public interface IAuthStore {
        IEnumerable<IAuthItem> GetAuthItems();
    }
}
