using System;
using System.Collections.Generic;
using System.Linq;

namespace Rey.Hunter.Verification.VerifyItems {
    public class EmptyVerifyItem<T> : VerifyItem {
        public IEnumerable<T> Values { get; }
        public EmptyVerifyItem(IEnumerable<T> values, Action failed)
            : base(failed) {
            this.Values = values;
        }

        public override bool Verify() {
            return this.Values.Count() == 0;
        }
    }

    public class NotEmptyVerifyItem<T> : EmptyVerifyItem<T> {
        public NotEmptyVerifyItem(IEnumerable<T> values, Action failed)
            : base(values, failed) {
        }

        public override bool Verify() {
            return !base.Verify();
        }
    }
}
