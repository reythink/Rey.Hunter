using MongoDB.Driver;
using Rey.Hunter.Models2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rey.Hunter.Repository {
    public interface IRepositoryManager {
        IAccountRepository Account();
        ICompanyRepository Company();
    }

    public interface IRepository<TModel> {
        void InsertOne(TModel model);
    }

    public interface IAccountRepository : IRepository<Account> {

    }

    public abstract class RepositoryBase<TModel> : IRepository<TModel> {
        public void InsertOne(TModel model) {
            throw new NotImplementedException();
        }
    }

    public class AccountRepository : RepositoryBase<Account>, IAccountRepository {

    }

    public class RepositoryManager : IRepositoryManager {
        private IMongoClient Client { get; }

        public RepositoryManager() {
            var settings = new MongoClientSettings() {
                Credentials = new List<MongoCredential> { MongoCredential.CreateCredential("admin", "admin", "admin123") }
            };

            this.Client = new MongoClient(settings);
        }

        public IAccountRepository Account() {
            return new AccountRepository();
        }

        public ICompanyRepository Company() {
            throw new NotImplementedException();
        }
    }
}
