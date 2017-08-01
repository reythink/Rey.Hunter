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
        private IMongoClient Client { get; }
        public IMongoDatabase Database { get; }
        public IMongoCollection<TModel> Collection { get; }
        public IMongoCollection<BsonDocument> BsonCollection { get; }

        public RepositoryBase(IRepositoryManager manager) {
            this.Manager = manager ?? throw new ArgumentNullException(nameof(manager));
            this.Client = this.Manager.Client;
            this.Database = this.Client.GetDatabase(this.GetDatabaseName());
            this.Collection = this.Database.GetCollection<TModel>(this.GetCollectionName());
            this.BsonCollection = this.Database.GetCollection<BsonDocument>(this.GetCollectionName());
        }

        protected virtual string GetDatabaseName() {
            return typeof(TModel).GetTypeInfo().GetCustomAttribute<MongoCollectionAttribute>()?.Database ?? this.Manager.DefaultDatabaseName;
        }

        protected virtual string GetCollectionName() {
            return typeof(TModel).GetTypeInfo().GetCustomAttribute<MongoCollectionAttribute>()?.Collection;
        }

        public virtual void InsertOne(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Collection.InsertOne(model);
        }

        public virtual void InsertMany(IEnumerable<TModel> models) {
            if (models == null)
                throw new ArgumentNullException(nameof(models));

            this.Collection.InsertMany(models);
        }

        public virtual void ReplaceOne(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.Id == null)
                throw new ArgumentNullException(nameof(model.Id));

            model.UpdateModifyAt();
            this.Collection.ReplaceOne(x => x.Id.Equals(model.Id), model);
        }

        public virtual void DeleteOne(string id) {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            this.Collection.DeleteOne(x => x.Id.Equals(id));
        }

        public void DeleteMany(IEnumerable<string> idList) {
            if (idList == null)
                throw new ArgumentNullException(nameof(idList));

            this.Collection.DeleteMany(Builders<TModel>.Filter.In(x => x.Id, idList));
        }

        public virtual TModel FindOne(string id) {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return this.Collection.Find(x => x.Id.Equals(id)).SingleOrDefault();
        }

        public virtual IEnumerable<TModel> FindAll() {
            return this.Collection.Find(x => true).ToEnumerable();
        }

        public virtual void Drop() {
            this.Database.DropCollection(this.GetCollectionName());
        }

        public abstract void UpdateRef(TModel model);
    }
}
