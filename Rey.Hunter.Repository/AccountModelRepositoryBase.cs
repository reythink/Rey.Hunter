using MongoDB.Driver;
using Rey.Hunter.Models2;
using System;
using System.Collections.Generic;

namespace Rey.Hunter.Repository {
    public abstract class AccountModelRepositoryBase<TModel> : RepositoryBase<TModel>, IAccountModelRepository<TModel>
        where TModel : class, IAccountModel {
        public Account Account { get; }

        public AccountModelRepositoryBase(IRepositoryManager manager, Account account)
            : base(manager) {
            this.Account = account;
        }

        public override void InsertOne(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.Account = this.Account;
            base.InsertOne(model);
        }

        public override void InsertMany(IEnumerable<TModel> models) {
            if (models == null)
                throw new ArgumentNullException(nameof(models));

            foreach (var model in models) {
                model.Account = this.Account;
            }

            base.InsertMany(models);
        }

        public override void ReplaceOne(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.Account = this.Account;
            base.ReplaceOne(model);
        }

        public override IEnumerable<TModel> FindAll() {
            return this.Collection.Find(x => x.Account.Equals(this.Account)).ToEnumerable();
        }
    }
}
