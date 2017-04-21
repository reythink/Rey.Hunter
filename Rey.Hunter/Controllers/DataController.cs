using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rey.Hunter.Models.Basic;
using Rey.Hunter.Models.Business;
using System.Linq;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class DataController : ReyController {
        public IActionResult Index() {
            return View();
        }

        public IActionResult Industries() {
            return View();
        }

        public IActionResult Functions() {
            return View();
        }

        public IActionResult Locations() {
            return View();
        }

        public IActionResult Categories() {
            return View();
        }

        public IActionResult Channels() {
            return View();
        }
    }
}
