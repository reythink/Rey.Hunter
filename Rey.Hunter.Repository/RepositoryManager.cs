using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Rey.Hunter.Repository.Repositories;
using System.Collections.Generic;

namespace Rey.Hunter.Repository {
    public class RepositoryManager : IRepositoryManager {
        public IMongoClient Client { get; }
        public string DefaultDatabaseName { get; } = "rey_test";

        public RepositoryManager() {
            ConventionRegistry.Register("CamelCase", new ConventionPack { new CamelCaseElementNameConvention() }, type => true);

            var settings = new MongoClientSettings() {
                Credentials = new List<MongoCredential> { MongoCredential.CreateCredential("admin", "admin", "admin123~") }
            };
            this.Client = new MongoClient(settings.Freeze());
        }

        public IAccountRepository Account() {
            return new AccountRepository(this);
        }

        public IRoleRepository Role(string accountId) {
            return new RoleRepository(this, accountId);
        }

        public IUserRepository User(string accountId) {
            return new UserRepository(this, accountId);
        }

        public IIndustryRepository Industry(string accountId) {
            return new IndustryRepository(this, accountId);
        }

        public ICompanyRepository Company(string accountId) {
            return new CompanyRepository(this, accountId);
        }
    }
}
