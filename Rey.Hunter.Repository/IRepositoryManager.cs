using MongoDB.Driver;
using System;
using System.Text;

namespace Rey.Hunter.Repository {
    public interface IRepositoryManager {
        IMongoClient Client { get; }

        IAccountRepository Account();
        IRoleRepository Role(string accountId);
        IUserRepository User(string accountId);

        ICompanyRepository Company(string accountId);
    }
}
