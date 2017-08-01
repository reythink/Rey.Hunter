using Rey.Hunter.Models2;
using System.Collections.Generic;

namespace Rey.Hunter.Repository {
    public interface IAccountNodeRepository<TModel, TModelRef> : IAccountRepository<TModel>
        where TModel : class, IAccountModel, INodeModel<TModel, TModelRef>
        where TModelRef : class, INodeModelRef {
        IEnumerable<TModel> Path(string id);
        IEnumerable<TModel> Path(TModel model);
    }
}
