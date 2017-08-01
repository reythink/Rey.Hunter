using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Rey.Hunter.Models2;
using Rey.Hunter.Repository.Auth;
using Rey.Hunter.Repository.Business;
using Rey.Hunter.Repository.Data;
using System;
using System.Collections.Generic;

namespace Rey.Hunter.Repository {
    public class RepositoryManager : IRepositoryManager {
        private Dictionary<Type, IRepository> Repositories { get; } = new Dictionary<Type, IRepository>();

        public IMongoClient Client { get; }
        public string DefaultDatabaseName { get; } = "rey_test";

        static RepositoryManager() {
            BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.LocalInstance);
            ConventionRegistry.Register("CamelCase", new ConventionPack { new CamelCaseElementNameConvention() }, type => true);
        }

        public RepositoryManager() {
            this.Client = new MongoClient(new MongoClientSettings() {
                Credentials = new List<MongoCredential> { MongoCredential.CreateCredential("admin", "admin", "admin123~") }
            }.Freeze());
        }

        public IRepository<TModel> Repository<TModel>()
            where TModel : class, IModel {
            return new DefaultRepository<TModel>(this);
        }

        public IAccountRepository<TModel> AccountRepository<TModel>(Account account)
            where TModel : class, IAccountModel {
            return new DefaultAccountRepository<TModel>(this, account);
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
