using Rey.Hunter.Models2;
using System;

namespace Rey.Hunter.Repository {
    public class DefaultRepository<TModel> : RepositoryBase<TModel>
        where TModel : class, IModel {
        public DefaultRepository(IRepositoryManager manager)
            : base(manager) {
        }

        public override void UpdateRef(TModel model) {
            throw new NotImplementedException();
        }
    }
}
