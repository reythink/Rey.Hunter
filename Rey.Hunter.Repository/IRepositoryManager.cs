using MongoDB.Driver;
using Rey.Hunter.Models2;
using Rey.Hunter.Repository.Auth;
using Rey.Hunter.Repository.Business;
using Rey.Hunter.Repository.Data;

namespace Rey.Hunter.Repository {
    public interface IRepositoryManager {
        IMongoClient Client { get; }
        string DefaultDatabaseName { get; }

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
