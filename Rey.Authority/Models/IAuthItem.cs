using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Authority.Models {
    public interface IAuthItem {
        IAuthTarget GetTarget();
        IEnumerable<IAuthActivity> GetActivities();
    }
}
