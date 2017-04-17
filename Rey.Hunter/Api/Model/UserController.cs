using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Web.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/")]
    public class UserController : ReyModelController<User, string> {
        public UserController() {
            this.Query = (query) => query.Where(x => x.Account.Id == this.CurrentAccount().Id);

            this.Create = (model) => {
                if (string.IsNullOrEmpty(model.Name))
                    throw new InvalidOperationException("Name cannot be empty");

                var password = (string)model.Data.password;
                if (string.IsNullOrEmpty(password))
                    throw new InvalidOperationException("Password cannot be null!");

                this.AttachCurrentAccount(model);
                this.LoginManager().SetPassword(model, password);
            };

            this.Update = (model, builder) => {
                if (string.IsNullOrEmpty(model.Name))
                    throw new InvalidOperationException("Name cannot be empty");

                var old = this.GetMonCollection<User>().FindOne(x => x.Id.Equals(model.Id));
                var update = builder.Set(x => x.Name, model.Name)
                                    .Set(x => x.Email, model.Email);

                if (!old.IsSuper) {
                    update = update
                        .Set(x => x.Enabled, model.Enabled)
                        .Set(x => x.Roles, model.Roles);
                }

                return update;
            };

            this.Delete = (model) => {
                if (model.IsSuper)
                    throw new InvalidOperationException("Cannot delete super role");
            };

            this.BatchDelete = (list) => {
                for (var i = list.Count() - 1; i >= 0; --i) {
                    var id = list[i];
                    if (this.GetMonCollection<User>().Exist(x => x.Id.Equals(id) && x.IsSuper)) {
                        list.RemoveAt(i);
                    }
                }
            };
        }
    }
}
