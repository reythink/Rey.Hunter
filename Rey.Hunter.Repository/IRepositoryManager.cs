using MongoDB.Driver;
using Rey.Hunter.Repository.Repositories;

namespace Rey.Hunter.Repository {
    public interface IRepositoryManager {
        IMongoClient Client { get; }
        string DefaultDatabaseName { get; }

        IAccountRepository Account();
        IRoleRepository Role(string accountId);
        IUserRepository User(string accountId);

        IIndustryRepository Industry(string accountId);

        ICompanyRepository Company(string accountId);
    }
}
