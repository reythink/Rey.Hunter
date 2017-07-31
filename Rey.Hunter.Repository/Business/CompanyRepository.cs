using System;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

namespace Rey.Hunter.Repository.Business {
    public class CompanyRepository : AccountModelRepositoryBase<Company>, ICompanyRepository {
        public CompanyRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public ICompanyQueryBuilder Query() {
            return new CompanyQueryBuilder(this);
        }

        public override void UpdateRef(Company model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var update = false;

            var account = this.Manager.Account().FindOne(model.Account.Id);
            if (account.ModifyAt != null) {
                if (model.Account.UpdateAt == null || account.ModifyAt > model.Account.UpdateAt) {
                    model.Account = account;
                    model.Account.UpdateAt = DateTime.Now;
                    if (!update) { update = true; }
                }
            }

            for (var i = 0; i < model.Industry.Count; ++i) {
                var item = model.Industry[i];
                var industry = this.Manager.Industry(this.Account).FindOne(item.Id);
                if (industry.ModifyAt != null) {
                    if (item.UpdateAt == null || industry.ModifyAt > item.UpdateAt) {
                        model.Industry[i] = industry;
                        model.Industry[i].UpdateAt = DateTime.Now;
                        if (!update) { update = true; }
                    }
                }
            }

            if (update) {
                this.ReplaceOne(model);
            }
        }
    }
}
