using System;
using System.Collections.Generic;
using System.Linq;

namespace Rey.MvcExtensions.Verification.VerifyItems {
    public class ContainVerifyItem<T> : VerifyItem {
        public IEnumerable<T> Values { get; }
        public T Value { get; }
        public ContainVerifyItem(IEnumerable<T> values, T value, Action failed)
            : base(failed) {
            this.Values = values;
            this.Value = value;
        }

        public override bool Verify() {
            return this.Values.Contains(this.Value);
        }
    }

    public class NotContainVerifyItem<T> : ContainVerifyItem<T> {
        public NotContainVerifyItem(IEnumerable<T> values, T value, Action failed)
            : base(values, value, failed) {
        }

        public override bool Verify() {
            return !base.Verify();
        }
    }
}
