using System;
using System.Collections.Generic;
using System.Linq;

namespace Rey.Hunter.Verification.VerifyItems {
    public class CountVerifyItem<T> : VerifyItem {
        public IEnumerable<T> Values { get; }
        public int Count { get; }
        public CountVerifyItem(IEnumerable<T> values, int count, Action failed)
            : base(failed) {
            this.Values = values;
            this.Count = count;
        }

        public override bool Verify() {
            return this.Values.Count() == this.Count;
        }
    }

    public class NotCountVerifyItem<T> : CountVerifyItem<T> {
        public NotCountVerifyItem(IEnumerable<T> values, int count, Action failed)
            : base(values, count, failed) {
        }

        public override bool Verify() {
            return !base.Verify();
        }
    }
}
