using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository {
    public interface IAccountRepository<TModel> : IRepository<TModel>
        where TModel : class, IAccountModel {
        Account Account { get; }
    }
}
