using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rey.Hunter.Models.Web.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class HomeController : ReyController {
        public IActionResult Index() {
            var db = this.GetMonDatabase();
            var talent = db.GetCollection<Talent>().FindOne(x => true);

            return View(talent);
        }
    }
}
