using Rey.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rey.Hunter.Models.Web.Identity;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class ProfileController : ReyController {
        private ILoginContext<User> LoginCtx { get; }

        public ProfileController(ILoginContext<User> loginCtx) {
            this.LoginCtx = loginCtx;
        }

        public IActionResult Index() {
            var user = this.LoginCtx.User;
            return View(user);
        }
    }
}
