using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rey.Hunter.Models.Business;
using System.Linq;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class CompanyController : ReyController {
        public IActionResult Index(string search,
            string[] name,
            string[] industry,
            string[] type,
            string[] status,
            string orderBy,
            string orderDirection,
            int page = 1) {

            var builder = new QueryBuilder<Company>(this.GetMonCollection<Company>());
            builder.FilterAccount(this.CurrentAccount().Id);
            builder.FilterSearch(search, x => x.Name);
            builder.FilterStringIn(x => x.Name, name, true);
            builder.FilterEnumIn(x => x.Type, type);
            builder.FilterEnumIn(x => x.Status, status);

            var query = builder.Build();

            if (industry != null && industry.Length > 0) {
                query = query.Where(x => x.Industries.Any(y => industry.Contains(y.Id)));
            }

            query = query.Order(orderBy, orderDirection);
            return View(query.Page(page, 15, (data) => this.ViewBag.PageData = data));
        }

        [HttpGet("/[controller]/{id}")]
        public IActionResult Item(string id) {
            var db = this.ViewBag.DB = this.GetMonDatabase();
            this.ViewBag.Logs = this.Logs<Company, string>(x => x.Model.Id.Equals(id))
                .OrderByDescending(x => x.Id)
                .Take(5)
                .Select(x => $"<div>{x.Action} By <strong>{x.User.Concrete(db)}</strong></div><div class=\"text-muted text-right\">{x.CreateAt}</div>");
            var model = this.GetMonCollection<Company>().FindOne(x => x.Id.Equals(id));
            return View(model);
        }
    }
}
