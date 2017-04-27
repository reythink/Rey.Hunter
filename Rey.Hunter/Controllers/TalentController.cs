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
using Rey.Mon.Models;
using Rey.Hunter.Models.Basic;

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

            this.ViewBag.DB = this.GetMonDatabase();

            var builder = new QueryBuilder<Talent>(this.GetMonCollection<Talent>());
            builder.FilterAccount(this.CurrentAccount().Id);
            builder.FilterSearch(search, x => x.EnglishName, x => x.ChineseName, x => x.Mobile, x => x.Phone, x => x.Email);
            builder.FilterStringIn(x => x.EnglishName, englishName, true);
            builder.FilterStringIn(x => x.ChineseName, chineseName, true);
            builder.FilterEnumIn(x => x.Gender, gender);

            var query = builder.Build();

            if (currentLocation != null && currentLocation.Length > 0) {
                query = query.Where(x => x.CurrentLocations.Any(y => currentLocation.Contains(y.Id)));
            }

            if (mobilityLocation != null && mobilityLocation.Length > 0) {
                query = query.Where(x => x.MobilityLocations.Any(y => mobilityLocation.Contains(y.Id)));
            }

            query = query.Order(orderBy, orderDirection);
            return View(query.Page(page, 15, (data) => this.ViewBag.PageData = data));
        }

        [HttpGet("/[controller]/{id}")]
        public IActionResult Item(string id) {
            var db = this.ViewBag.DB = this.GetMonDatabase();
            this.ViewBag.Logs = this.Logs<Talent, string>(x => x.Model.Id.Equals(id))
                .OrderByDescending(x => x.Id)
                .Take(5)
                .Select(x => $"<div>{x.Action} By <strong>{x.User.Concrete(db)}</strong></div><div class=\"text-muted text-right\">{x.CreateAt}</div>");
            var model = this.GetMonCollection<Talent>().FindOne(x => x.Id.Equals(id));
            return View(model);
        }
    }
}
