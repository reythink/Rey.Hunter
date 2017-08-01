using Rey.Hunter.Models2;
using System.Collections.Generic;
using System;

namespace Rey.Hunter.Repository {
    public abstract class AccountNodeRepositoryBase<TModel, TModelRef> : AccountRepositoryBase<TModel>, IAccountNodeRepository<TModel, TModelRef>
        where TModel : class, IAccountModel, INodeModel<TModel, TModelRef>
        where TModelRef : class, INodeModelRef {
        public AccountNodeRepositoryBase(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public IEnumerable<TModel> Path(string id) {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return this.Path(this.FindOne(id));
        }

        public IEnumerable<TModel> Path(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var results = new List<TModel>();
            var temp = model;
            while (temp != null) {
                results.Add(temp);

                if (temp.Parent != null && temp.Parent.Id != null) {
                    temp = this.FindOne(temp.Parent.Id);
                    continue;
                }

                temp = null;
            }
            return results;
        }
    }
}
