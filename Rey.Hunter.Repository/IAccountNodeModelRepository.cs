using Rey.Hunter.Models2;
using System.Collections.Generic;

namespace Rey.Hunter.Repository {
    public interface IAccountNodeModelRepository<TModel> : IAccountModelRepository<TModel>
        where TModel : class, IAccountModel, INodeModel {
        IEnumerable<TModel> Path(string id);
        IEnumerable<TModel> Path(TModel model);
    }
}
