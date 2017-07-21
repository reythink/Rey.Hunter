using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Rey.Hunter.Models.Business;
using Rey.Hunter.Query;
using Rey.Mon;
using System;
using System.Linq;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class TalentController : ReyController {
        public IActionResult Index(string search,
            string[] company,
            string[] previousCompany,
            string[] title,
            string[] inChargeOf,
            string[] grade,
            string[] industry,
            string[] crossIndustry,
            string[] function,
            string[] crossFunction,
            string[] crossCategory,
            string[] crossChannel,
            string[] brandsHadManaged,
            string[] KAHadManaged,
            string[] currentLocation,
            string[] mobilityLocation,
            int[] gender,
            int[] education,
            int[] language,
            int[] nationality,
            int[] jobIntension,
            string[] cv,
            string[] notes,
            string orderBy,
            string orderDirection,
            int page = 1) {

            var begin = DateTime.Now;
            IMonDatabase db = this.ViewBag.DB = this.GetMonDatabase();
            var query = new TalentAdvancedQuery2(db, this.CurrentAccount().Id)
                .Search(search)
                .Company(company)
                .PreviousCompany(previousCompany)
                .Title(title)
                .InChargeOf(inChargeOf)
                .Grade(grade)
                .Industry(industry)
                .CrossIndustry(crossIndustry)
                .Function(function)
                .CrossFunction(crossFunction)
                .CrossCategory(crossCategory)
                .CrossChannel(crossChannel)
                .BrandsHadManaged(brandsHadManaged)
                .KAHadManaged(KAHadManaged)
                .CurrentLocation(currentLocation)
                .MobilityLocation(mobilityLocation)
                .Gender(gender)
                .Education(education)
                .Language(language)
                .Nationality(nationality)
                .JobIntension(jobIntension)
                .CV(cv)
                .Notes(notes)
                .QueryModels().AsQueryable();

            if (!string.IsNullOrEmpty(orderBy)) {
                if (orderBy.Equals("Company", StringComparison.CurrentCultureIgnoreCase)) {
                    query = query.Order(x => x.Experiences.Find(y => y.CurrentJob == true)?.Company, x => x.Name, db, orderDirection);
                } else if (orderBy.Equals("Title", StringComparison.CurrentCultureIgnoreCase)) {
                    query = query.Order(x => x.Experiences.Find(y => y.CurrentJob == true)?.Title, orderDirection);
                } else if (orderBy.Equals("CurrentLocation", StringComparison.CurrentCultureIgnoreCase)) {
                    query = query.Order(x => x.CurrentLocations.FirstOrDefault(), x => x.Name, db, orderDirection);
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
            IMonDatabase db = this.ViewBag.DB = this.GetMonDatabase();
            this.ViewBag.Logs = this.Logs<Talent, string>(x => x.Model.Id.Equals(id))
                .OrderByDescending(x => x.Id)
                .Take(5)
                .Select(x => $"<div>{x.Action} By <strong>{x.User.Concrete(db)}</strong></div><div class=\"text-muted text-right\">{x.CreateAt}</div>");
            var model = this.GetMonCollection<Talent>().FindOne(x => x.Id.Equals(id));
            return View(model);
        }
    }
}
