using MongoDB.Driver;
using Rey.Hunter.Models2;
using System.Collections.Generic;

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
            var filter = FilterBuilder.Eq(x => x.Account.Key, this.AccountId);
            return this.Collection.Find(filter).ToEnumerable();
        }
    }
}
