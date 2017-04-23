using Rey.Hunter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.AspNetCore.Mvc {
    public class ReyAccountModelController<TModel> : ReyStringModelController<TModel>
        where TModel : AccountModel {
        public ReyAccountModelController() {
            this.BeforeQuery += query => query.Where(x => x.Account.Id == this.CurrentAccount().Id);

            this.BeforeCreate += model => {
                this.AttachCurrentAccount(model);
            };
        }
    }
}
