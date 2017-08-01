using MongoDB.Driver;
using Rey.Hunter.Models2;
using Rey.Hunter.Repository.Auth;
using Rey.Hunter.Repository.Business;
using Rey.Hunter.Repository.Data;

namespace Rey.Hunter.Repository {
    public interface IRepositoryManager {
        IMongoClient Client { get; }
        string DefaultDatabaseName { get; }

        IRepository<TModel> Repository<TModel>()
            where TModel : class, IModel;

        IAccountRepository<TModel> AccountRepository<TModel>(Account account)
            where TModel : class, IAccountModel;

        #region Auth

        IAccountRepository Account();
        IRoleRepository Role(Account account);
        IUserRepository User(Account account);

        #endregion

        #region Data

        IIndustryRepository Industry(Account account);
        IFunctionRepository Function(Account account);
        ILocationRepository Location(Account account);
        ICategoryRepository Category(Account account);
        IChannelRepository Channel(Account account);

        #endregion

        #region Business

        ICompanyRepository Company(Account account);
        ITalentRepository Talent(Account account);
        IProjectRepository Project(Account account);

        #endregion
    }
}
