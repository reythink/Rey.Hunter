using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Rey.Hunter.Models2;
using System.Collections.Generic;
using System;

namespace Rey.Hunter.Repository {
    public interface IRoleRepository : IAccountModelRepository<Role> {

    }

    public class RoleRepository : AccountModelRepositoryBase<Role>, IRoleRepository {
        public RoleRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }

        public override string GetCollectionName() {
            return "role";
        }
    }

    public interface IUserRepository : IAccountModelRepository<User> {

    }

    public class UserRepository : AccountModelRepositoryBase<User>, IUserRepository {
        public UserRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }

        public override string GetCollectionName() {
            return "user";
        }
    }

    public class RepositoryManager : IRepositoryManager {
        public IMongoClient Client { get; }

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

        public ICompanyRepository Company(string accountId) {
            return new CompanyRepository(this, accountId);
        }
    }
}
