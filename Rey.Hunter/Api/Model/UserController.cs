using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/")]
    public class UserController : ReyAccountModelController<User> {
        public UserController() {
            this.BeforeSearch += (query, search) => {
                return query.Where(x =>
                x.Name.ToLower().Contains(search.ToLower()) ||
                x.Email.Contains(search)
                );
            };

            this.BeforeCreate += (user) => {
                this.LoginManager().SetPassword(user, user.Password);
            };
        }
    }
}
