using System;
using System.Collections.Generic;
using Rey.Hunter.Models2;
using MongoDB.Driver;

namespace Rey.Hunter.Repository {
    public abstract class AccountModelRepositoryBase<TModel> : RepositoryBase<TModel>, IAccountModelRepository<TModel>
        where TModel : class, IAccountModel {
        public string AccountId { get; }

        public AccountModelRepositoryBase(IRepositoryManager manager, string accountId)
            : base(manager) {
            this.AccountId = accountId;
        }

        public override void InsertOne(TModel model) {
            model.Account = this.AccountId;
            base.InsertOne(model);
        }

        public virtual IEnumerable<TModel> FindAll() {
            var filter = FilterBuilder.Eq(x => x.Account.Id, this.AccountId);
            return this.GetCollection().Find(filter).ToEnumerable();
        }
    }
}
