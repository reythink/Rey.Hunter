using MongoDB.Driver;
using Rey.Hunter.Repository.Auth;
using Rey.Hunter.Repository.Business;
using Rey.Hunter.Repository.Data;

namespace Rey.Hunter.Repository {
    public interface IRepositoryManager {
        IMongoClient Client { get; }
        string DefaultDatabaseName { get; }

        #region Auth

        IAccountRepository Account();
        IRoleRepository Role(string accountId);
        IUserRepository User(string accountId);

        #endregion

        #region Data

        IIndustryRepository Industry(string accountId);
        IFunctionRepository Function(string accountId);
        ILocationRepository Location(string accountId);
        ICategoryRepository Category(string accountId);
        IChannelRepository Channel(string accountId);

        #endregion

        #region Business

        ICompanyRepository Company(string accountId);
        ITalentRepository Talent(string accountId);
        IProjectRepository Project(string accountId);

        #endregion
    }
}
