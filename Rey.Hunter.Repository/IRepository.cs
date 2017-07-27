using MongoDB.Driver;
using System.Collections.Generic;

namespace Rey.Hunter.Repository {
    public interface IRepository<TModel> {
        void InsertOne(TModel model);
        void InsertMany(IEnumerable<TModel> models);

        void ReplaceOne(TModel model);

        void DeleteOne(string id);
        void DeleteMany(IEnumerable<string> list);

        TModel FindOne(string id);
        void Drop();

        IQueryBuilder<TModel> Query();
    }
}
