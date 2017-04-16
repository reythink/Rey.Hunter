using Rey.Hunter.Verification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc {
    public static class VerifierExceptionExtensions {
        public static IVerifier Is(this IVerifier target, Func<bool> verifier, Exception exception) {
            return target.Is(verifier, () => { throw exception; });
        }

        public static IVerifier IsTrue(this IVerifier target, bool value, Exception exception) {
            return target.IsTrue(value, () => { throw exception; });
        }

        public static IVerifier IsFalse(this IVerifier target, bool value, Exception exception) {
            return target.IsFalse(value, () => { throw exception; });
        }

        public static IVerifier IsNull(this IVerifier target, object value, Exception exception) {
            return target.IsNull(value, () => { throw exception; });
        }

        public static IVerifier IsNotNull(this IVerifier target, object value, Exception exception) {
            return target.IsNotNull(value, () => { throw exception; });
        }

        public static IVerifier IsEqual(this IVerifier target, object value1, object value2, Exception exception) {
            return target.IsEqual(value1, value2, () => { throw exception; });
        }

        public static IVerifier IsNotEqual(this IVerifier target, object value1, object value2, Exception exception) {
            return target.IsNotEqual(value1, value2, () => { throw exception; });
        }

        public static IVerifier IsEqual<T>(this IVerifier target, T value1, T value2, Exception exception)
            where T : IEquatable<T> {
            return target.IsEqual(value1, value2, () => { throw exception; });
        }

        public static IVerifier IsNotEqual<T>(this IVerifier target, T value1, T value2, Exception exception)
            where T : IEquatable<T> {
            return target.IsNotEqual(value1, value2, () => { throw exception; });
        }

        public static IVerifier IsStringEqual(this IVerifier target, string value1, string value2, bool ignoreCase, Exception exception) {
            return target.IsStringEqual(value1, value2, ignoreCase, () => { throw exception; });
        }

        public static IVerifier IsStringNotEqual(this IVerifier target, string value1, string value2, bool ignoreCase, Exception exception) {
            return target.IsStringNotEqual(value1, value2, ignoreCase, () => { throw exception; });
        }

        public static IVerifier IsStringEqual(this IVerifier target, string value1, string value2, Exception exception) {
            return target.IsStringEqual(value1, value2, () => { throw exception; });
        }

        public static IVerifier IsStringNotEqual(this IVerifier target, string value1, string value2, Exception exception) {
            return target.IsStringNotEqual(value1, value2, () => { throw exception; });
        }

        public static IVerifier IsStringNullOrEmpty(this IVerifier target, string value, Exception exception) {
            return target.IsStringNullOrEmpty(value, () => { throw exception; });
        }

        public static IVerifier IsStringNotNullOrEmpty(this IVerifier target, string value, Exception exception) {
            return target.IsStringNotNullOrEmpty(value, () => { throw exception; });
        }

        public static IVerifier IsRegex(this IVerifier target, string value, Regex ex, Exception exception) {
            return target.IsRegex(value, ex, () => { throw exception; });
        }

        public static IVerifier IsNotRegex(this IVerifier target, string value, Regex ex, Exception exception) {
            return target.IsNotRegex(value, ex, () => { throw exception; });
        }

        public static IVerifier IsEmpty<T>(this IVerifier target, IEnumerable<T> values, Exception exception) {
            return target.IsEmpty(values, () => { throw exception; });
        }

        public static IVerifier IsNotEmpty<T>(this IVerifier target, IEnumerable<T> values, Exception exception) {
            return target.IsNotEmpty(values, () => { throw exception; });
        }

        public static IVerifier IsCount<T>(this IVerifier target, IEnumerable<T> values, int count, Exception exception) {
            return target.IsCount(values, count, () => { throw exception; });
        }

        public static IVerifier IsNotCount<T>(this IVerifier target, IEnumerable<T> values, int count, Exception exception) {
            return target.IsNotCount(values, count, () => { throw exception; });
        }

        public static IVerifier IsContain<T>(this IVerifier target, IEnumerable<T> values, T value, Exception exception) {
            return target.IsContain(values, value, () => { throw exception; });
        }

        public static IVerifier IsNotContain<T>(this IVerifier target, IEnumerable<T> values, T value, Exception exception) {
            return target.IsNotContain(values, value, () => { throw exception; });
        }

        public static IVerifier IsMobile(this IVerifier target, string mobile, Exception exception) {
            return target.IsMobile(mobile, () => { throw exception; });
        }

        public static IVerifier IsNotMobile(this IVerifier target, string mobile, Exception exception) {
            return target.IsNotMobile(mobile, () => { throw exception; });
        }
    }
}
