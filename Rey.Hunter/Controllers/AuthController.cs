using Rey.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rey.Hunter.Models.Web.Identity;
using System.Linq;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class AuthController : ReyController {
        private ILoginContext<User> LoginCtx { get; }

        public AuthController(ILoginContext<User> loginCtx) {
            this.LoginCtx = loginCtx;
        }

        public IActionResult Roles(string search, int page = 1) {
            var query = this.GetMonCollection<Role>().Query()
                .Where(x => x.Account.Id.Equals(this.CurrentAccount().Id));

            if (!string.IsNullOrEmpty(search)) {
                query = query.Where(x => x.Name.ToLower().Contains(search.ToLower()));
            }

            query = query.OrderByDescending(x => x.Id);
            return View(query.Page(page, 15, (data) => this.ViewBag.PageData = data));
        }

        public IActionResult Users(string search, int page = 1) {
            var query = this.GetMonCollection<User>().Query()
                .Where(x => x.Account.Id.Equals(this.CurrentAccount().Id));

            if (!string.IsNullOrEmpty(search)) {
                query = query.Where(x => x.Name.ToLower().Contains(search.ToLower())
                || x.Email.ToLower().Contains(search.ToLower()));
            }

            query = query.OrderByDescending(x => x.Id);
            return View(query.Page(page, 15, (data) => this.ViewBag.PageData = data));
        }
    }
}
