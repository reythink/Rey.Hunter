using Microsoft.AspNetCore.Mvc;
using Rey.Hunter.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Api.Mise {
    [Route("/Api/[controller]/[action]")]
    public class SecurityController : ReyController {
        [HttpPost]
        public Task<IActionResult> SavePassword([FromBody]PasswordEntity password) {
            return this.JsonInvokeAsync(() => {
                if (password == null)
                    throw new InvalidOperationException("No data!");

                if (string.IsNullOrEmpty(password.Old))
                    throw new InvalidOperationException("Old password cannot be empty!");

                if (string.IsNullOrEmpty(password.New))
                    throw new InvalidOperationException("New password cannot be empty!");

                if (string.IsNullOrEmpty(password.Confirm))
                    throw new InvalidOperationException("Confirm password cannot be empty!");

                if (!password.New.Equals(password.Confirm))
                    throw new InvalidOperationException("Different confirm password!");

                var userId = password.UserId ?? this.CurrentUser().Id;
                var collection = this.GetMonCollection<User>();
                var user = collection.FindOne(x => x.Id.Equals(userId));

                if (!this.LoginManager().VerifyPassword(user, password.Old))
                    throw new InvalidOperationException("Invalid old password!");

                collection.ReplaceOne(x => x.Id.Equals(user.Id),
                    this.LoginManager().SetPassword(user, password.New));
            });
        }

        public class PasswordEntity {
            public string UserId { get; set; }
            public string Old { get; set; }
            public string New { get; set; }
            public string Confirm { get; set; }
        }
    }
}
