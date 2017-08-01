using Rey.Hunter.Models2;
using System;

namespace Rey.Hunter.Repository {
    public class DefaultAccountRepository<TModel> : AccountRepositoryBase<TModel>
        where TModel : class, IAccountModel {
        public DefaultAccountRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public override void UpdateRef(TModel model) {
            throw new NotImplementedException();
        }
    }
}
