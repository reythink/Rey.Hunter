using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rey.Hunter.Models.Web.Business;
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
            builder.AddAccountFilter(this.CurrentAccount().Id);
            builder.AddSearchFilter(search, x => x.Name);
            builder.AddStringInFilter(x => x.Name, name, true);
            builder.AddStringInFilter(x => x.Industries, x => x.Id, industry);
            builder.AddEnumInFilter(x => x.Type, type);
            builder.AddEnumInFilter(x => x.Status, status);

            var query = builder.Build().Order(orderBy, orderDirection);
            return View(query.Page(page, 15, (data) => this.ViewBag.PageData = data));
        }

        [HttpGet("/[controller]/{id}")]
        public IActionResult Item(string id) {
            var model = this.GetMonCollection<Company>().FindOne(x => x.Id.Equals(id));
            return View(model);
        }
    }
}
