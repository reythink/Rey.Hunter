using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Verification {
    public interface IVerifyItem {
        Action Failed { get; }
        bool Verify();
    }
}
