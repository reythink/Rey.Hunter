using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class HomeController : ReyController {
        public IActionResult Index() {
            return View();
        }
    }
}
