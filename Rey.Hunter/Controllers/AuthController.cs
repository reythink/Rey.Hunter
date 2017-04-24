using Rey.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rey.Hunter.Models.Identity;
using System.Linq;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class AuthController : ReyController {
        public IActionResult Roles(string search,
            string[] name,
            string orderBy,
            string orderDirection,
            int page = 1) {

            var builder = new QueryBuilder<Role>(this.GetMonCollection<Role>());
            builder.FilterAccount(this.CurrentAccount().Id);
            builder.FilterSearch(search, x => x.Name);
            builder.FilterStringIn(x => x.Name, name, true);

            var query = builder.Build().Order(orderBy, orderDirection);
            return View(query.Page(page, 15, (data) => this.ViewBag.PageData = data));
        }

        public IActionResult Users(string search,
            string[] name,
            string orderBy,
            string orderDirection,
            int page = 1) {

            var builder = new QueryBuilder<User>(this.GetMonCollection<User>());
            builder.FilterAccount(this.CurrentAccount().Id);
            builder.FilterSearch(search, x => x.Name);
            builder.FilterStringIn(x => x.Name, name, true);

            var query = builder.Build().Order(orderBy, orderDirection);
            return View(query.Page(page, 15, (data) => this.ViewBag.PageData = data));
        }
    }
}
