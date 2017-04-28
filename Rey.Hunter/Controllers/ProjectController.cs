using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using Rey.Hunter.Models.Business;
using Rey.Hunter.Query;
using Rey.Mon;
using System;
using System.Linq;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class ProjectController : ReyController {
        public IActionResult Index(string search,
             string[] name,
             string[] client,
             string[] function,
             string[] manager,
             string[] consultant,
             string orderBy,
             string orderDirection,
             int page = 1) {

            IMonDatabase db = this.ViewBag.DB = this.GetMonDatabase();
            var query = new ProjectAdvancedQuery(db, this.CurrentAccount().Id)
                .Search(search)
                .Name(name)
                .Client(client)
                .Function(function)
                .Manager(manager)
                .Consultant(consultant)
                .Query;

            if (!string.IsNullOrEmpty(orderBy)) {
                if (orderBy.Equals("Client", StringComparison.CurrentCultureIgnoreCase)) {
                    query = query.Order(x => x.Client, x => x.Name, db, orderDirection);
                } else if (orderBy.Equals("Function", StringComparison.CurrentCultureIgnoreCase)) {
                    query = query.Order(x => x.Functions.FirstOrDefault(), x => x.Name, db, orderDirection);
                } else if (orderBy.Equals("Manager", StringComparison.CurrentCultureIgnoreCase)) {
                    query = query.Order(x => x.Manager, x => x.Name, db, orderDirection);
                } else {
                    query = query.Order(orderBy, orderDirection);
                }
            } else {
                query = query.OrderByDescending(x => x.Id);
            }
            return View(query.Page(page, 15, (data) => this.ViewBag.PageData = data));
        }

        [HttpGet("/Project/{id}")]
        public IActionResult Item(string id) {
            var db = this.ViewBag.DB = this.GetMonDatabase();
            this.ViewBag.Logs = this.Logs<Project, string>(x => x.Model.Id.Equals(id))
                .OrderByDescending(x => x.Id)
                .Take(5)
                .Select(x => $"<div>{x.Action} By <strong>{x.User.Concrete(db)}</strong></div><div class=\"text-muted text-right\">{x.CreateAt}</div>");
            var model = this.GetMonCollection<Project>().FindOne(x => x.Id.Equals(id));
            return View(model);
        }
    }
}
