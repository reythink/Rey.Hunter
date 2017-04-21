using Rey.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Identity;
using System.Threading.Tasks;

namespace Rey.Hunter.Api {
    [Authorize]
    [Route("/Api/[controller]/")]
    public class CurrentController : ReyController {
        private ILoginContext<User> LoginCtx { get; }

        public CurrentController(ILoginContext<User> loginCtx) {
            this.LoginCtx = loginCtx;
        }

        [Route("portrait")]
        public Task<IActionResult> Portrait() {
            return this.JsonInvokeOneAsync(() => {
                var user = this.LoginCtx.User;
                return user.PortraitUrl;
            });
        }

        [HttpPost("portrait")]
        public Task<IActionResult> Portrait(string url) {
            return this.JsonInvokeAsync(() => {
                var user = this.LoginCtx.User;
                this.GetMonCollection<User>().UpdateOne(x => x.Id.Equals(user.Id), (builder) => {
                    return builder.Set(x => x.PortraitUrl, url);
                });
            });
        }
    }
}
