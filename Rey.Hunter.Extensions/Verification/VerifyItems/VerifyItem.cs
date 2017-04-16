using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Verification.VerifyItems {
    public abstract class VerifyItem : IVerifyItem {
        public Action Failed { get; }
        public VerifyItem(Action failed) {
            this.Failed = failed;
        }

        public abstract bool Verify();
    }
}
