using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using Rey.Hunter.Models.Web.Business;
using System.Linq;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class ProjectController : ReyController {
        public IActionResult Index(string search,
             string[] name,
             int page = 1) {

            var builder = new QueryBuilder<Project>(this.GetMonCollection<Project>());
            builder.AddAccountFilter(this.CurrentAccount().Id);
            builder.AddSearchFilter(search, x => x.Name);
            builder.AddStringInFilter(x => x.Name, name, true);

            var query = builder.Build().OrderByDescending(x => x.Id);
            return View(query.Page(page, 15, (data) => this.ViewBag.PageData = data));
        }

        [HttpGet("/Project/{id}")]
        public IActionResult Item(string id) {
            this.ViewBag.DB = this.GetMonDatabase();
            var model = this.GetMonCollection<Project>().FindOne(x => x.Id.Equals(id));
            return View(model);
        }
    }
}
