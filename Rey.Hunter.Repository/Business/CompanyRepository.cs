using System;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Business {
    public class CompanyRepository : AccountRepositoryBase<Company>, ICompanyRepository {
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

            var account = this.Manager.Repository<Account>().FindOne(model.Account.Id);
            if (RefNeedUpdate(account, model.Account)) {
                model.Account = account;
                model.Account.UpdateAt = DateTime.Now;
                if (!update) { update = true; }
            }

            for (var i = 0; i < model.Industry.Count; ++i) {
                var item = model.Industry[i];
                var industry = this.Manager.AccountRepository<Industry>(this.Account).FindOne(item.Id);
                if (RefNeedUpdate(industry, model.Industry[i])) {
                    model.Industry[i] = industry;
                    model.Industry[i].UpdateAt = DateTime.Now;
                    if (!update) { update = true; }
                }
            }

            if (update) {
                this.ReplaceOne(model);
            }
        }

        private static bool RefNeedUpdate(IModel model, IModelRef modelRef) {
            if (model.ModifyAt == null)
                return false;

            if (modelRef.UpdateAt == null
                || model.ModifyAt > modelRef.UpdateAt)
                return true;

            return false;
        }

        private static void RefUpdate<TModel, TModelRef>(TModelRef modelRef)
            where TModel : class, IModel
            where TModelRef : class, IModelRef {

        }
    }
}
