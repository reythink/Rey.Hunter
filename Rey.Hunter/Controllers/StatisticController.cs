using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class StatisticController : ReyController {
        public IActionResult Index() {
            return View();
        }
    }
}
