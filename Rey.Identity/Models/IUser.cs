using System.Collections.Generic;
using System.Security.Claims;

namespace Rey.Identity.Models {
    public interface IUser {
        IEnumerable<Claim> GetLoginClaims();
        string GetSalt();
        void SetSalt(string salt);
        string GetPassword();
        void SetPassword(string password);
    }
}
