using Rey.Identity.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Rey.Identity.Services {
    public class LoginContext<TUser> : ILoginContext<TUser>
        where TUser : class, IUser {
        private IHttpContextAccessor HttpContext { get; }
        private IUserStore<TUser> UserStore { get; }

        public TUser User {
            get { return this.UserStore.GetUserByClaims(this.HttpContext.HttpContext.User.Claims); }
        }

        public LoginContext(IHttpContextAccessor httpContext,
            IUserStore<TUser> userStore) {
            this.HttpContext = httpContext;
            this.UserStore = userStore;
        }
    }
}
