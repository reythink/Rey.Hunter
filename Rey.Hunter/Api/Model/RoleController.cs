using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Web.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/")]
    public class RoleController : ReyModelController<Role, string> {
        public RoleController() {
            this.Query = (query) => query.Where(x => x.Account.Id == this.CurrentAccount().Id);

            this.Create = (model) => {
                if (string.IsNullOrEmpty(model.Name))
                    throw new InvalidOperationException("Name cannot be empty");

                this.AttachCurrentAccount(model);
            };

            this.Update = (model, builder) => {
                if (string.IsNullOrEmpty(model.Name))
                    throw new InvalidOperationException("Name cannot be empty");

                var old = this.GetMonCollection<Role>().FindOne(x => x.Id.Equals(model.Id));
                var update = builder.Set(x => x.Name, model.Name);

                if (!old.IsSuper) {
                    update = update.Set(x => x.AuthItems, model.AuthItems)
                                    .Set(x => x.Enabled, model.Enabled);
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
                    if (this.GetMonCollection<Role>().Exist(x => x.Id.Equals(id) && x.IsSuper)) {
                        list.RemoveAt(i);
                    }
                }
            };
        }
    }
}
