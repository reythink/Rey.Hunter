using System;

namespace Rey.MvcExtensions.Verification.VerifyItems {
    public class NullVerifyItem : VerifyItem {
        public object Value { get; }
        public NullVerifyItem(object value, Action failed)
            : base(failed) {
            this.Value = value;
        }

        public override bool Verify() {
            return this.Value == null;
        }
    }

    public class NotNullVerifyItem : NullVerifyItem {
        public NotNullVerifyItem(object value, Action failed)
            : base(value, failed) {
        }

        public override bool Verify() {
            return !base.Verify();
        }
    }
}
