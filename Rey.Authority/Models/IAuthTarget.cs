using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Authority.Models {
    public interface IAuthTarget {
        string Name { get; }
        string DisplayName { get; }
    }
}
