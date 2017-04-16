using System;
using System.Text.RegularExpressions;

namespace Rey.MvcExtensions.Verification.VerifyItems {
    public class RegexVerifyItem : VerifyItem {
        public string Value { get; }
        public Regex Ex { get; }
        public RegexVerifyItem(string value, Regex ex, Action failed)
            : base(failed) {
            this.Value = value;
            this.Ex = ex;
        }

        public override bool Verify() {
            return this.Ex.Match(this.Value).Success;
        }
    }

    public class NotRegexVerifyItem : RegexVerifyItem {
        public NotRegexVerifyItem(string value, Regex ex, Action failed)
            : base(value, ex, failed) {
        }

        public override bool Verify() {
            return !base.Verify();
        }
    }
}
