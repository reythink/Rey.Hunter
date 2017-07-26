using Rey.Hunter.Models2;
using System.Collections.Generic;

namespace Rey.Hunter.Repository {
    public interface IAccountModelRepository<TModel> : IRepository<TModel>
        where TModel : class, IAccountModel {
        string AccountId { get; }
        IEnumerable<TModel> FindAll();
    }
}
