using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.MvcExtensions.Verification.VerifyItems {
    public class StringNullOrEmptyVerifyItem : VerifyItem {
        public string Value { get; }
        public StringNullOrEmptyVerifyItem(string value, Action failed)
            : base(failed) {
            this.Value = value;
        }

        public override bool Verify() {
            return string.IsNullOrEmpty(this.Value);
        }
    }

    public class StringNotNullOrEmptyVerifyItem : StringNullOrEmptyVerifyItem {
        public StringNotNullOrEmptyVerifyItem(string value, Action failed)
            : base(value, failed) {
        }

        public override bool Verify() {
            return !base.Verify();
        }
    }
}
