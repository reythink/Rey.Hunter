using System;

namespace Rey.MvcExtensions.Verification.VerifyItems {
    public class StringEqualVerifyItem : VerifyItem {
        public string Value1 { get; }
        public string Value2 { get; }
        public bool IgnoreCase { get; }
        public StringEqualVerifyItem(string value1, string value2, bool ignoreCase, Action failed)
            : base(failed) {
            this.Value1 = value1;
            this.Value2 = value2;
            this.IgnoreCase = ignoreCase;
        }

        public override bool Verify() {
            return string.Equals(this.Value1, this.Value2,
                this.IgnoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
        }
    }

    public class StringNotEqualVerifyItem : StringEqualVerifyItem {
        public StringNotEqualVerifyItem(string value1, string value2, bool ignoreCase, Action failed)
            : base(value1, value2, ignoreCase, failed) {
        }

        public override bool Verify() {
            return !base.Verify();
        }
    }
}
