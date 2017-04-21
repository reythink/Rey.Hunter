using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rey.Hunter.Models.Business;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class HomeController : ReyController {
        public IActionResult Index() {
            var count = this.ViewBag.Count = new ExpandoObject();
            count.Talent = this.GetMonCollection<Talent>().Count(x => x.Account.Id.Equals(this.CurrentAccount().Id));
            count.Company = this.GetMonCollection<Company>().Count(x => x.Account.Id.Equals(this.CurrentAccount().Id));
            count.Project = this.GetMonCollection<Project>().Count(x => x.Account.Id.Equals(this.CurrentAccount().Id));
            return View();
        }
    }
}
