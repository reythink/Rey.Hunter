using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rey.Mon {
    public interface IMonCollection<TModel> {
        IMonDatabase Database { get; }
        IMongoCollection<TModel> MongoCollection { get; }

        void Drop();

        void InsertOne(TModel model);
        void InsertMany(IEnumerable<TModel> models);

        void ReplaceOne(Expression<Func<TModel, bool>> filter, TModel model);

        void UpdateOne(Expression<Func<TModel, bool>> filter, Func<UpdateDefinitionBuilder<TModel>, UpdateDefinition<TModel>> update);
        void UpdateMany(Expression<Func<TModel, bool>> filter, Func<UpdateDefinitionBuilder<TModel>, UpdateDefinition<TModel>> update);

        void DeleteOne(Expression<Func<TModel, bool>> filter);
        void DeleteMany(Expression<Func<TModel, bool>> filter);

        IFindFluent<TModel, TModel> Find(Expression<Func<TModel, bool>> filter);
        TModel FindOne(Expression<Func<TModel, bool>> filter);
        IEnumerable<TModel> FindMany(Expression<Func<TModel, bool>> filter);

        IMongoQueryable<TModel> Query();

        long Count(Expression<Func<TModel, bool>> filter);
        bool Exist(Expression<Func<TModel, bool>> filter);
    }
}
