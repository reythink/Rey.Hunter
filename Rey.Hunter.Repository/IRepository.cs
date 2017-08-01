using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Rey.Hunter.Repository {
    public interface IRepository {
        IRepositoryManager Manager { get; }
        IMongoDatabase Database { get; }
    }

    public interface IRepository<TModel> : IRepository {
        IMongoCollection<TModel> Collection { get; }
        IMongoCollection<BsonDocument> BsonCollection { get; }

        void InsertOne(TModel model);
        void InsertMany(IEnumerable<TModel> models);
        void InsertMany(params TModel[] models);

        void ReplaceOne(TModel model);

        void DeleteOne(string id);
        void DeleteMany(IEnumerable<string> idList);
        void DeleteMany(params string[] idList);

        TModel FindOne(string id);
        IEnumerable<TModel> FindAll();

        void Drop();

        void UpdateRef(TModel model);
    }
}
