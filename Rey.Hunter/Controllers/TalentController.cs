using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using Rey.Hunter.Models.Business;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Bson;
using Rey.Hunter.Models;
using Rey.Mon;
using System.Reflection;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class TalentController : ReyController {
        public IActionResult Index(string search,
            string[] englishName,
            string[] chineseName,
            string[] currentLocation,
            string[] mobilityLocation,
            string[] gender,
            string orderBy,
            string orderDirection,
            int page = 1) {

            var builder = new QueryBuilder<Talent>(this.GetMonCollection<Talent>());
            builder.AddAccountFilter(this.CurrentAccount().Id);
            builder.AddSearchFilter(search, x => x.EnglishName, x => x.ChineseName, x => x.Mobile, x => x.Phone, x => x.Email);
            builder.AddStringInFilter(x => x.EnglishName, englishName, true);
            builder.AddStringInFilter(x => x.ChineseName, chineseName, true);
            builder.AddStringInFilter(x => x.CurrentLocations, x => x.Id, currentLocation);
            builder.AddStringInFilter(x => x.MobilityLocations, x => x.Id, mobilityLocation);
            builder.AddEnumInFilter(x => x.Gender, gender);

            var query = builder.Build().Order(orderBy, orderDirection);
            return View(query.Page(page, 15, (data) => this.ViewBag.PageData = data));
        }

        [HttpGet("/[controller]/{id}")]
        public IActionResult Item(string id) {
            var db = this.GetMonDatabase();
            this.ViewBag.Logs = this.Logs<Talent, string>(x => x.Model.Id.Equals(id))
                .OrderByDescending(x => x.Id)
                .Take(5)
                .Select(x => $"<div>{x.Action} By <strong>{x.User.Concrete(db)}</strong></div><div class=\"text-muted text-right\">{x.CreateAt}</div>");
            var model = this.GetMonCollection<Talent>().FindOne(x => x.Id.Equals(id));
            return View(model);
        }
    }
}
