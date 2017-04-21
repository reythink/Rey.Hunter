using Microsoft.AspNetCore.Mvc;
using Rey.Hunter.Models.Identity;
using Rey.Mon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Controllers {
    public class AccountController : ReyController {
        public IActionResult Login(string returnUrl) {
            return View();
        }

        [HttpPost]
        public Task<IActionResult> Login(string email, string password, string returnUrl) {
            return this.InvokeAsync(async () => {
                if (string.IsNullOrEmpty(email))
                    throw new Exception("EMail cannot be empty!");

                if (string.IsNullOrEmpty(password))
                    throw new Exception("Password cannot be empty!");

                var user = this.GetMonCollection<User>().FindOne(x => x.Email.Equals(email));
                if (user == null)
                    throw new Exception("Invalid email address!");

                if (!this.LoginManager().VerifyPassword(user, password))
                    throw new Exception("Invalid password!");

                if (!user.Enabled)
                    throw new Exception("User disabled!");

                await this.LoginManager().LoginAsync(user);
                return Redirect(returnUrl ?? "/");
            }, (ex) => {
                this.ViewBag.Error = ex.Message;
                return View();
            });
        }

        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public Task<IActionResult> Register(string company, string name, string email, string password, string returnUrl) {
            return this.InvokeAsync(async () => {
                if (string.IsNullOrEmpty(company))
                    throw new Exception("Company cannot be empty!");

                if (string.IsNullOrEmpty(name))
                    throw new Exception("Name cannot be empty!");

                if (string.IsNullOrEmpty(email))
                    throw new Exception("EMail cannot be null!");

                if (string.IsNullOrEmpty(password))
                    throw new Exception("Password cannot be null!");

                if (this.GetMonCollection<User>().Count(x => x.Email.Equals(email)) > 0)
                    throw new Exception("EMail registered!");

                var user = this.LoginManager().Register(new Account { Company = company }, (account) => {
                    return new Role {
                        Account = account,
                        Name = "Administrators",
                        IsSuper = true
                    };
                }, (account, role) => {
                    return this.LoginManager().SetPassword(new User {
                        Account = account,
                        Email = email,
                        Name = name,
                        IsSuper = true,
                        Roles = new List<MonStringModelRef<Role>> { role }
                    }, password);
                });

                await this.LoginManager().LoginAsync(user);
                return Redirect(returnUrl ?? "/Admin");
            }, (ex) => {
                this.ViewBag.Error = ex.Message;
                return View();
            });
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl) {
            await this.LoginManager().LogoutAsync();
            return Redirect(returnUrl ?? "/");
        }
    }
}
