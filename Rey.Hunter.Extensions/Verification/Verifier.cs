using Rey.Hunter.Verification.VerifyItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rey.Hunter.Verification {
    public class Verifier : IVerifier {
        public static readonly Regex REGEX_MOBILE = new Regex("^0?1\\d{10}$");

        public List<IVerifyItem> Items { get; } = new List<IVerifyItem>();

        public IVerifier Is(IVerifyItem item) {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            this.Items.Add(item);
            return this;
        }

        public IVerifier Is(Func<bool> verifier, Action failed) {
            if (verifier == null)
                throw new ArgumentNullException(nameof(verifier));

            if (failed == null)
                throw new ArgumentNullException(nameof(failed));

            return this.Is(new DefaultValueVerifyItem(verifier, failed));
        }

        public IVerifier IsTrue(bool value, Action failed) {
            return Is(new TrueVerifyItem(value, failed));
        }

        public IVerifier IsFalse(bool value, Action failed) {
            return Is(new FalseVerifyItem(value, failed));
        }

        public IVerifier IsNull(object value, Action failed) {
            return Is(new NullVerifyItem(value, failed));
        }

        public IVerifier IsNotNull(object value, Action failed) {
            return Is(new NotNullVerifyItem(value, failed));
        }

        public IVerifier IsEqual(object value1, object value2, Action failed) {
            return Is(new EqualVerifyItem(value1, value2, failed));
        }

        public IVerifier IsNotEqual(object value1, object value2, Action failed) {
            return Is(new NotEqualVerifyItem(value1, value2, failed));
        }

        public IVerifier IsEqual<T>(T value1, T value2, Action failed)
            where T : IEquatable<T> {
            return Is(new HEqualVerifyItem<T>(value1, value2, failed));
        }

        public IVerifier IsNotEqual<T>(T value1, T value2, Action failed)
            where T : IEquatable<T> {
            return Is(new HNotEqualVerifyItem<T>(value1, value2, failed));
        }

        public IVerifier IsStringEqual(string value1, string value2, bool ignoreCase, Action failed) {
            return Is(new StringEqualVerifyItem(value1, value2, ignoreCase, failed));
        }

        public IVerifier IsStringNotEqual(string value1, string value2, bool ignoreCase, Action failed) {
            return Is(new StringNotEqualVerifyItem(value1, value2, ignoreCase, failed));
        }

        public IVerifier IsStringEqual(string value1, string value2, Action failed) {
            return Is(new StringEqualVerifyItem(value1, value2, false, failed));
        }

        public IVerifier IsStringNotEqual(string value1, string value2, Action failed) {
            return Is(new StringNotEqualVerifyItem(value1, value2, false, failed));
        }

        public IVerifier IsStringNullOrEmpty(string value, Action failed) {
            return Is(new StringNullOrEmptyVerifyItem(value, failed));
        }

        public IVerifier IsStringNotNullOrEmpty(string value, Action failed) {
            return Is(new StringNotNullOrEmptyVerifyItem(value, failed));
        }

        public IVerifier IsRegex(string value, Regex ex, Action failed) {
            return Is(new RegexVerifyItem(value, ex, failed));
        }

        public IVerifier IsNotRegex(string value, Regex ex, Action failed) {
            return Is(new NotRegexVerifyItem(value, ex, failed));
        }

        public IVerifier IsEmpty<T>(IEnumerable<T> values, Action failed) {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Is(new EmptyVerifyItem<T>(values, failed));
        }

        public IVerifier IsNotEmpty<T>(IEnumerable<T> values, Action failed) {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Is(new NotEmptyVerifyItem<T>(values, failed));
        }

        public IVerifier IsCount<T>(IEnumerable<T> values, int count, Action failed) {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Is(new CountVerifyItem<T>(values, count, failed));
        }

        public IVerifier IsNotCount<T>(IEnumerable<T> values, int count, Action failed) {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Is(new NotCountVerifyItem<T>(values, count, failed));
        }

        public IVerifier IsContain<T>(IEnumerable<T> values, T value, Action failed) {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Is(new ContainVerifyItem<T>(values, value, failed));
        }

        public IVerifier IsNotContain<T>(IEnumerable<T> values, T value, Action failed) {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Is(new NotContainVerifyItem<T>(values, value, failed));
        }

        public IVerifier IsMobile(string mobile, Action failed) {
            return IsRegex(mobile, REGEX_MOBILE, failed);
        }

        public IVerifier IsNotMobile(string mobile, Action failed) {
            return IsNotRegex(mobile, REGEX_MOBILE, failed);
        }

        public void Verify() {
            foreach (var item in this.Items) {
                if (!item.Verify()) {
                    item.Failed();
                }
            }
        }
    }
}
