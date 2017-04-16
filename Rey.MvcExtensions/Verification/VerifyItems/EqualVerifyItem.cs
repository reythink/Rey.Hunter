using System;

namespace Rey.MvcExtensions.Verification.VerifyItems {
    public class EqualVerifyItem : VerifyItem {
        public object Value1 { get; }
        public object Value2 { get; }
        public EqualVerifyItem(object value1, object value2, Action failed)
            : base(failed) {
            this.Value1 = value1;
            this.Value2 = value2;
        }

        public override bool Verify() {
            return object.Equals(this.Value1, this.Value2);
        }
    }

    public class NotEqualVerifyItem : EqualVerifyItem {
        public NotEqualVerifyItem(object value1, object value2, Action failed)
            : base(value1, value2, failed) {
        }

        public override bool Verify() {
            return !base.Verify();
        }
    }

    public class HEqualVerifyItem<T> : VerifyItem
        where T : IEquatable<T> {
        public T Value1 { get; }
        public T Value2 { get; }
        public HEqualVerifyItem(T value1, T value2, Action failed)
            : base(failed) {
            this.Value1 = value1;
            this.Value2 = value2;
        }

        public override bool Verify() {
            if (this.Value1 == null && this.Value2 == null)
                return true;

            if (this.Value1 == null || this.Value2 == null)
                return false;

            return this.Value1.Equals(this.Value2);
        }
    }

    public class HNotEqualVerifyItem<T> : HEqualVerifyItem<T>
        where T : IEquatable<T> {
        public HNotEqualVerifyItem(T value1, T value2, Action failed)
            : base(value1, value2, failed) {
        }

        public override bool Verify() {
            return !base.Verify();
        }
    }
}
