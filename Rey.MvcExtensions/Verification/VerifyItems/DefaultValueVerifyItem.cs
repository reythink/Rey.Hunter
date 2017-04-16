using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.MvcExtensions.Verification.VerifyItems {
    public class DefaultValueVerifyItem : VerifyItem {
        public Func<bool> Verifier { get; }
        public DefaultValueVerifyItem(Func<bool> verifier, Action failed)
            : base(failed) {
            this.Verifier = verifier;
        }

        public override bool Verify() {
            if (this.Verifier == null)
                throw new InvalidOperationException($"{nameof(this.Verifier)} cannot be null!");

            return this.Verifier();
        }
    }
}
