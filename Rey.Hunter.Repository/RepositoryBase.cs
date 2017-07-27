using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rey.Hunter.Repository {
    public abstract class RepositoryBase<TModel> : IRepository<TModel>
        where TModel : class, IModel {
        public IRepositoryManager Manager { get; }
        public IMongoClient Client => this.Manager.Client;
        public IMongoDatabase Database => this.Client.GetDatabase(this.GetDatabaseName());
        public IMongoCollection<TModel> Collection => this.Database.GetCollection<TModel>(this.GetCollectionName());
        public IMongoCollection<BsonDocument> DocCollectin => this.Database.GetCollection<BsonDocument>(this.GetCollectionName());

        protected static FilterDefinitionBuilder<TModel> FilterBuilder => Builders<TModel>.Filter;
        protected static IndexKeysDefinitionBuilder<TModel> IndexKeysBuilder => Builders<TModel>.IndexKeys;
        protected static ProjectionDefinitionBuilder<TModel> ProjectionBuilder => Builders<TModel>.Projection;
        protected static SortDefinitionBuilder<TModel> SortBuilder => Builders<TModel>.Sort;
        protected static UpdateDefinitionBuilder<TModel> UpdateBuilder => Builders<TModel>.Update;

        public RepositoryBase(IRepositoryManager manager) {
            this.Manager = manager;
        }

        protected virtual string GetDatabaseName() {
            return typeof(TModel).GetTypeInfo().GetCustomAttribute<MongoCollectionAttribute>()?.Database ?? this.Manager.DefaultDatabaseName;
        }

        protected virtual string GetCollectionName() {
            return typeof(TModel).GetTypeInfo().GetCustomAttribute<MongoCollectionAttribute>()?.Collection;
        }

        public virtual void InsertOne(TModel model) {
            this.Collection.InsertOne(model);
        }

        public virtual void InsertMany(IEnumerable<TModel> models) {
            this.Collection.InsertMany(models);
        }

        public virtual void ReplaceOne(TModel model) {
            this.Collection.ReplaceOne(x => x.Id.Equals(model.Id), model);
        }

        public virtual void DeleteOne(string id) {
            this.Collection.DeleteOne(x => x.Id.Equals(id));
        }

        public void DeleteMany(IEnumerable<string> list) {
            var filter = FilterBuilder.In(x => x.Id, list);
            this.Collection.DeleteMany(filter);
        }

        public virtual TModel FindOne(string id) {
            return this.Collection.Find(x => x.Id.Equals(id)).SingleOrDefault();
        }

        public virtual void Drop() {
            this.Database.DropCollection(this.GetCollectionName());
        }

        //public IQueryBuilder<TModel> Query() {
        //    //var filter = Builders<BsonDocument>.Filter.Regex("industry.name", new BsonRegularExpression("edu", "ig"));

        //    ////this.Collection.Indexes.CreateOne(IndexKeysBuilder.Ascending("name"));
        //    ////var count = this.Collection.Find(x => true).Count();

        //    //var agg = this.Collection.Aggregate()
        //    //    .Lookup("industry", "industry.@id", "_id", "industry")
        //    //    .Match(filter);

        //    ////var count = agg.Group("{_id: 1, result : { '$sum' : 1 }}").SingleOrDefault();
        //    ////var count = agg.Count().SingleOrDefault()?.Count ?? 0;
        //    //var list = agg
        //    //    .Skip((1 - 1) * 15)
        //    //    .Limit(10)
        //    //    .ToEnumerable();


        //    return new QueryBuilder<TModel>(this);
        //}
    }
}
