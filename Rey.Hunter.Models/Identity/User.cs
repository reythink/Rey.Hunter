using Rey.Identity.Models;
using Rey.Mon.Attributes;
using Rey.Mon.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System;

namespace Rey.Hunter.Models.Identity {
    [MonCollection("ide.users")]
    public class User : AccountModel, IUser {
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool IsSuper { get; set; }
        public bool Enabled { get; set; } = true;
        public List<MonStringModelRef<Role>> Roles { get; set; } = new List<MonStringModelRef<Role>>();
        public string PortraitUrl { get; set; } = "/img/avatar.png";
        public string Position { get; set; }

        public IEnumerable<Claim> GetLoginClaims() {
            return new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, this.Id.ToString()),
                new Claim(ClaimTypes.Email, this.Email),
                new Claim(ClaimTypes.Name, this.Name)
            };
        }

        public string GetSalt() {
            return this.Salt;
        }

        public void SetSalt(string salt) {
            this.Salt = salt;
        }

        public string GetPassword() {
            return this.Password;
        }

        public void SetPassword(string password) {
            this.Password = password;
        }

        public override string ToString() {
            return this.Name ?? this.Email;
        }
    }
}
