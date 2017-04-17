using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class DevelopController : ReyController {
        public IActionResult Tree() {
            return View();
        }
    }
}
