using System;
using MongoDB.Driver;
using Rey.Hunter.Models2;
using System.Collections.Generic;

namespace Rey.Hunter.Repository {
    public abstract class RepositoryBase<TModel> : IRepository<TModel>
        where TModel : class, IModel {
        protected IRepositoryManager Manager { get; }
        protected IMongoClient Client => this.Manager.Client;

        protected static FilterDefinitionBuilder<TModel> FilterBuilder => Builders<TModel>.Filter;
        protected static IndexKeysDefinitionBuilder<TModel> IndexKeysBuilder => Builders<TModel>.IndexKeys;
        protected static ProjectionDefinitionBuilder<TModel> ProjectionBuilder => Builders<TModel>.Projection;
        protected static SortDefinitionBuilder<TModel> SortBuilder => Builders<TModel>.Sort;
        protected static UpdateDefinitionBuilder<TModel> UpdateBuilder => Builders<TModel>.Update;

        public RepositoryBase(IRepositoryManager manager) {
            this.Manager = manager;
        }

        public virtual string GetDatabaseName() {
            return "rey_test";
        }

        public abstract string GetCollectionName();

        protected virtual IMongoDatabase GetDatabase() {
            return this.Client.GetDatabase(this.GetDatabaseName());
        }

        protected virtual IMongoCollection<TModel> GetCollection() {
            return this.GetDatabase().GetCollection<TModel>(this.GetCollectionName());
        }

        public virtual void InsertOne(TModel model) {
            this.GetCollection().InsertOne(model);
        }

        public virtual void ReplaceOne(TModel model) {
            this.GetCollection().ReplaceOne(x => x.Id.Equals(model.Id), model);
        }

        public virtual void DeleteOne(string id) {
            this.GetCollection().DeleteOne(x => x.Id.Equals(id));
        }

        public virtual void Drop() {
            this.GetDatabase().DropCollection(this.GetCollectionName());
        }
    }
}
