using MongoDB.Driver;
using Rey.Hunter.Models2;
using System;
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
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.AccountId = this.AccountId;
            base.InsertOne(model);
        }

        public override void InsertMany(IEnumerable<TModel> models) {
            if (models == null)
                throw new ArgumentNullException(nameof(models));

            foreach (var model in models) {
                model.AccountId = this.AccountId;
            }

            base.InsertMany(models);
        }

        public override void ReplaceOne(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.AccountId = this.AccountId;
            base.ReplaceOne(model);
        }

        public override IEnumerable<TModel> FindAll() {
            return this.Collection.Find(x => x.AccountId.Equals(this.AccountId)).ToEnumerable();
        }
    }
}
