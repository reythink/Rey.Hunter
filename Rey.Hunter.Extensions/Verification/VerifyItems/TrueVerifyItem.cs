using System;

namespace Rey.Hunter.Verification.VerifyItems {
    public class TrueVerifyItem : VerifyItem {
        public bool Value { get; }
        public TrueVerifyItem(bool value, Action failed)
            : base(failed) {
            this.Value = value;
        }

        public override bool Verify() {
            return this.Value == true;
        }
    }

    public class FalseVerifyItem : TrueVerifyItem {
        public FalseVerifyItem(bool value, Action failed)
            : base(value, failed) {
        }

        public override bool Verify() {
            return !base.Verify();
        }
    }
}
