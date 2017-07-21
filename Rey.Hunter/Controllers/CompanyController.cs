using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rey.Hunter.Models.Business;
using Rey.Hunter.Query;
using Rey.Mon;
using System;
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

            var begin = DateTime.Now;
            IMonDatabase db = this.ViewBag.DB = this.GetMonDatabase();
            var query = new CompanyAdvancedQuery(db, this.CurrentAccount().Id)
                .Search(search)
                .Name(name)
                .Industry(industry)
                .Type(type)
                .Status(status)
                .Query;

            if (!string.IsNullOrEmpty(orderBy)) {
                if (orderBy.Equals("Industry", StringComparison.CurrentCultureIgnoreCase)) {
                    query = query.Order(x => x.Industries.FirstOrDefault(), x => x.Name, db, orderDirection);
                } else {
                    query = query.Order(orderBy, orderDirection);
                }
            } else {
                query = query.OrderByDescending(x => x.Id);
            }

            var models = query.Page(page, 15, (data) => this.ViewBag.PageData = data);
            this.ViewBag.Elapsed = (DateTime.Now - begin).TotalSeconds;
            return View(models);
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
