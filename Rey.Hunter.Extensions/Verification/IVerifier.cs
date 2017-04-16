using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rey.Hunter.Verification {
    public interface IVerifier {
        IVerifier Is(IVerifyItem item);
        IVerifier Is(Func<bool> verifier, Action failed);

        IVerifier IsTrue(bool value, Action failed);
        IVerifier IsFalse(bool value, Action failed);

        IVerifier IsNull(object value, Action failed);
        IVerifier IsNotNull(object value, Action failed);

        IVerifier IsEqual(object value1, object value2, Action failed);
        IVerifier IsNotEqual(object value1, object value2, Action failed);

        IVerifier IsEqual<T>(T value1, T value2, Action failed)
            where T : IEquatable<T>;
        IVerifier IsNotEqual<T>(T value1, T value2, Action failed)
            where T : IEquatable<T>;

        IVerifier IsStringEqual(string value1, string value2, bool ignoreCase, Action failed);
        IVerifier IsStringNotEqual(string value1, string value2, bool ignoreCase, Action failed);

        IVerifier IsStringEqual(string value1, string value2, Action failed);
        IVerifier IsStringNotEqual(string value1, string value2, Action failed);

        IVerifier IsStringNullOrEmpty(string value, Action failed);
        IVerifier IsStringNotNullOrEmpty(string value, Action failed);

        IVerifier IsRegex(string value, Regex ex, Action failed);
        IVerifier IsNotRegex(string value, Regex ex, Action failed);

        IVerifier IsEmpty<T>(IEnumerable<T> values, Action failed);
        IVerifier IsNotEmpty<T>(IEnumerable<T> values, Action failed);

        IVerifier IsCount<T>(IEnumerable<T> values, int count, Action failed);
        IVerifier IsNotCount<T>(IEnumerable<T> values, int count, Action failed);

        IVerifier IsContain<T>(IEnumerable<T> values, T value, Action failed);
        IVerifier IsNotContain<T>(IEnumerable<T> values, T value, Action failed);

        IVerifier IsMobile(string mobile, Action failed);
        IVerifier IsNotMobile(string mobile, Action failed);

        void Verify();
    }
}
