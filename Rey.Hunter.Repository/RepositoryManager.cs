using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Rey.Hunter.Repository.Auth;
using Rey.Hunter.Repository.Business;
using Rey.Hunter.Repository.Data;
using System.Collections.Generic;
using System;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository {
    public class RepositoryManager : IRepositoryManager {
        private Dictionary<Type, IRepository> Repositories { get; } = new Dictionary<Type, IRepository>();

        public IMongoClient Client { get; }
        public string DefaultDatabaseName { get; } = "rey_test";

        public RepositoryManager() {
            BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.LocalInstance);
            ConventionRegistry.Register("CamelCase", new ConventionPack { new CamelCaseElementNameConvention() }, type => true);

            this.Client = new MongoClient(new MongoClientSettings() {
                Credentials = new List<MongoCredential> { MongoCredential.CreateCredential("admin", "admin", "admin123~") }
            }.Freeze());
        }

        #region Auth

        public IAccountRepository Account() {
            return new AccountRepository(this);
        }

        public IRoleRepository Role(Account account) {
            return new RoleRepository(this, account);
        }

        public IUserRepository User(Account account) {
            return new UserRepository(this, account);
        }

        #endregion

        #region Data

        public IIndustryRepository Industry(Account account) {
            return new IndustryRepository(this, account);
        }

        public IFunctionRepository Function(Account account) {
            return new FunctionRepository(this, account);
        }

        public ILocationRepository Location(Account account) {
            return new LocationRepository(this, account);
        }

        public ICategoryRepository Category(Account account) {
            return new CategoryRepository(this, account);
        }

        public IChannelRepository Channel(Account account) {
            return new ChannelRepository(this, account);
        }

        #endregion

        #region Business

        public ICompanyRepository Company(Account account) {
            return new CompanyRepository(this, account);
        }

        public ITalentRepository Talent(Account account) {
            return new TalentRepository(this, account);
        }

        public IProjectRepository Project(Account account) {
            return new ProjectRepository(this, account);
        }

        #endregion
    }
}
