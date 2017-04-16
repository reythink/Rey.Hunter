using Rey.Identity.Configuration;
using Rey.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Rey.Identity.Services {
    public class LoginManager<TUser> : ILoginManager<TUser>
        where TUser : class, IUser {
        protected IdentityOptions Options { get; }
        protected IHttpContextAccessor HttpContext { get; }
        protected IUserStore<TUser> UserStore { get; }

        public LoginManager(
            IdentityOptions options,
            IHttpContextAccessor httpContext,
            IUserStore<TUser> userStore) {
            this.Options = options;
            this.HttpContext = httpContext;
            this.UserStore = userStore;
        }

        public async Task LoginAsync(TUser user) {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            await LoginAsync(user, null);
        }

        public async Task LoginAsync(TUser user, AuthenticationProperties properties) {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var claims = user.GetLoginClaims();
            if (claims == null)
                throw new InvalidOperationException("Cannot get claims from user");

            if (!claims.Any(x => x.Type == ClaimTypes.NameIdentifier))
                throw new InvalidOperationException("Claims of login user must include identifier claim.");

            var identity = new ClaimsIdentity(new List<Claim>(claims), this.Options.Scheme);
            var principal = new ClaimsPrincipal(identity);

            if (properties == null) {
                await this.HttpContext.HttpContext.Authentication.SignInAsync(this.Options.Scheme, principal);
            } else {
                await this.HttpContext.HttpContext.Authentication.SignInAsync(this.Options.Scheme, principal, properties);
            }
        }

        public async Task LogoutAsync() {
            await LogoutAsync(null);
        }

        public async Task LogoutAsync(AuthenticationProperties properties) {
            if (properties == null) {
                await this.HttpContext.HttpContext.Authentication.SignOutAsync(this.Options.Scheme);
            } else {
                await this.HttpContext.HttpContext.Authentication.SignOutAsync(this.Options.Scheme, properties);
            }
        }

        public TUser SetPassword(TUser user, string password) {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var salt = GenerateSalt();
            user.SetSalt(salt);
            user.SetPassword(HashPassword(password, salt));

            return user;
        }

        public bool VerifyPassword(TUser user, string password) {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var hashed = user.GetPassword();
            var salt = user.GetSalt();
            return HashPassword(password, salt).Equals(hashed);
        }

        protected string GenerateSalt() {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        protected string HashPassword(string password, string salt) {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            byte[] hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(hashed);
        }

        public TUser Register(TUser user) {
            throw new NotImplementedException();
        }
    }

    public class LoginManager<TUser, TRole> : LoginManager<TUser>, ILoginManager<TUser, TRole>
        where TUser : class, IUser
        where TRole : class, IRole {
        protected IRoleStore<TRole> RoleStore { get; }
        public LoginManager(
            IdentityOptions options,
            IHttpContextAccessor httpContext,
            IUserStore<TUser> userStore,
            IRoleStore<TRole> roleStore)
            : base(options, httpContext, userStore) {
            this.RoleStore = roleStore;
        }

        public TUser Register(TRole role, Func<TRole, TUser> configUser) {
            throw new NotImplementedException();
        }
    }

    public class LoginManager<TUser, TRole, TAccount> : LoginManager<TUser, TRole>, ILoginManager<TUser, TRole, TAccount>
        where TUser : class, IUser
        where TRole : class, IRole
        where TAccount : class, IAccount {
        protected IAccountStore<TAccount> AccountStore { get; }
        public LoginManager(
            IdentityOptions options,
            IHttpContextAccessor httpContext,
            IUserStore<TUser> userStore,
            IRoleStore<TRole> roleStore,
            IAccountStore<TAccount> accountStore)
            : base(options, httpContext, userStore, roleStore) {
            this.AccountStore = accountStore;
        }

        public TUser Register(TAccount account, Func<TAccount, TRole> configRole, Func<TAccount, TRole, TUser> configUser) {
            this.AccountStore.InsertOne(account);
            var role = configRole(account);
            this.RoleStore.InsertOne(role);
            var user = configUser(account, role);
            this.UserStore.InsertOne(user);
            return user;
        }
    }
}
