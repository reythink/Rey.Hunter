using Rey.Authority.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/")]
    public class AuthController : ReyController {
        private IAuthStore AuthStore { get; }

        public AuthController(IAuthStore authStore) {
            this.AuthStore = authStore;
        }

        [Route("Items")]
        public Task<IActionResult> Items() {
            return this.JsonInvokeManyAsync(() => {
                return this.AuthStore.GetAuthItems();
            });
        }
    }
}
