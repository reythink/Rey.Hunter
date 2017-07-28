using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Rey.Hunter.Repository.Auth;
using Rey.Hunter.Repository.Business;
using Rey.Hunter.Repository.Data;
using System.Collections.Generic;
using System;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;

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

        public IRoleRepository Role(string accountId) {
            return new RoleRepository(this, accountId);
        }

        public IUserRepository User(string accountId) {
            return new UserRepository(this, accountId);
        }

        #endregion

        #region Data

        public IIndustryRepository Industry(string accountId) {
            return new IndustryRepository(this, accountId);
        }

        public IFunctionRepository Function(string accountId) {
            return new FunctionRepository(this, accountId);
        }

        public ILocationRepository Location(string accountId) {
            return new LocationRepository(this, accountId);
        }

        public ICategoryRepository Category(string accountId) {
            return new CategoryRepository(this, accountId);
        }

        public IChannelRepository Channel(string accountId) {
            return new ChannelRepository(this, accountId);
        }

        #endregion

        #region Business

        public ICompanyRepository Company(string accountId) {
            return new CompanyRepository(this, accountId);
        }

        public ITalentRepository Talent(string accountId) {
            return new TalentRepository(this, accountId);
        }

        public IProjectRepository Project(string accountId) {
            return new ProjectRepository(this, accountId);
        }

        #endregion
    }
}
