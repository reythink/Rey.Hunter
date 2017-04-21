using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Business;
using System;
using System.Linq;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/")]
    public class CompanyGroupController : ReyModelController<CompanyGroup, string> {
        public CompanyGroupController() {
            this.Query = (query) => query.Where(x => x.Account.Id == this.CurrentAccount().Id);

            this.Create = (model) => {
                if (string.IsNullOrEmpty(model.Name))
                    throw new InvalidOperationException("Name cannot be empty");

                this.AttachCurrentAccount(model);
            };

            this.Update = (model, builder) => {
                if (string.IsNullOrEmpty(model.Name))
                    throw new InvalidOperationException("Name cannot be empty");

                return builder.Set(x => x.Name, model.Name);
            };
        }
    }
}
