using MongoDB.Driver;
using System.Collections.Generic;

namespace Rey.Hunter.Repository {
    public interface IRepository<TModel> {
        string GetDatabaseName();
        string GetCollectionName();

        IMongoDatabase GetDatabase();
        IMongoCollection<TModel> GetCollection();

        void InsertOne(TModel model);
        void InsertMany(IEnumerable<TModel> models);

        void ReplaceOne(TModel model);

        void DeleteOne(string id);
        void DeleteMany(IEnumerable<string> list);

        TModel FindOne(string id);
        void Drop();

        IQueryBuilder<TModel> Query();
    }

    public interface IQueryBuilder<TModel> {

    }

    public class QueryBuilder<TModel> : IQueryBuilder<TModel> {
        private IRepository<TModel> Repository { get; }

        public QueryBuilder(IRepository<TModel> repository) {
            this.Repository = repository;
        }
    }
}
