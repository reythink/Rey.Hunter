﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using Rey.Hunter.Models.Business;
using System.Linq;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class ProjectController : ReyController {
        public IActionResult Index(string search,
             string[] name,
             string orderBy,
             string orderDirection,
             int page = 1) {

            var builder = new QueryBuilder<Project>(this.GetMonCollection<Project>());
            builder.AddAccountFilter(this.CurrentAccount().Id);
            builder.AddSearchFilter(search, x => x.Name);
            builder.AddStringInFilter(x => x.Name, name, true);

            var query = builder.Build().Order(orderBy, orderDirection);
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
