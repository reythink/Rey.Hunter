using Rey.Mon.Attributes;
using Rey.Mon.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using MongoDB.Driver.GridFS;

namespace Rey.Mon {
    public class MonDatabase : IMonDatabase {
        public IMonClient Client { get; }
        public IMongoDatabase MongoDatabase { get; }

        public MonDatabase(IMonClient client, IMongoDatabase mongoDatabase) {
            this.Client = client;
            this.MongoDatabase = mongoDatabase;
        }

        public void Drop() {
            this.Client.DropDatabase(this.MongoDatabase.DatabaseNamespace.DatabaseName);
        }

        public void CreateCollection(string name) {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.MongoDatabase.CreateCollection(name);
        }

        public void DropCollection(string name) {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.MongoDatabase.DropCollection(name);
        }

        public void RenameCollection(string oldName, string newName) {
            if (oldName == null)
                throw new ArgumentNullException(nameof(oldName));

            if (newName == null)
                throw new ArgumentNullException(nameof(newName));

            this.MongoDatabase.RenameCollection(oldName, newName);
        }

        #region GetCollection

        public IMonCollection<TModel> GetCollection<TModel>(IMongoCollection<TModel> mongoCollection) {
            if (mongoCollection == null)
                throw new ArgumentNullException(nameof(mongoCollection));

            return new MonCollection<TModel>(this, mongoCollection);
        }

        public IMonCollection<TModel> GetCollection<TModel>(string name) {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return this.GetCollection(this.MongoDatabase.GetCollection<TModel>(name));
        }

        public IMonCollection<TModel> GetCollection<TModel>() {
            return this.GetCollection<TModel>(GetCollectionName<TModel>());
        }

        #endregion GetCollection

        #region GetRepository

        public IMonRepository<TModel, TKey> GetRepository<TModel, TKey>(IMongoCollection<TModel> mongoCollection)
            where TModel : class, IMonModel<TKey> {
            if (mongoCollection == null)
                throw new ArgumentNullException(nameof(mongoCollection));

            return new MonRepository<TModel, TKey>(this, mongoCollection);
        }

        public IMonRepository<TModel, TKey> GetRepository<TModel, TKey>(string name)
            where TModel : class, IMonModel<TKey> {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return this.GetRepository<TModel, TKey>(this.MongoDatabase.GetCollection<TModel>(name));
        }

        public IMonRepository<TModel, TKey> GetRepository<TModel, TKey>()
            where TModel : class, IMonModel<TKey> {
            return this.GetRepository<TModel, TKey>(GetCollectionName<TModel>());
        }

        public IMonRepository<TModel, ObjectId> GetRepository<TModel>(IMongoCollection<TModel> mongoCollection)
            where TModel : class, IMonModel<ObjectId> {
            if (mongoCollection == null)
                throw new ArgumentNullException(nameof(mongoCollection));

            return new MonRepository<TModel, ObjectId>(this, mongoCollection);
        }

        public IMonRepository<TModel, ObjectId> GetRepository<TModel>(string name)
            where TModel : class, IMonModel<ObjectId> {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return this.GetRepository<TModel, ObjectId>(this.MongoDatabase.GetCollection<TModel>(name));
        }

        public IMonRepository<TModel, ObjectId> GetRepository<TModel>()
            where TModel : class, IMonModel<ObjectId> {
            return this.GetRepository<TModel, ObjectId>(GetCollectionName<TModel>());
        }

        #endregion GetRepository

        #region GetNodeRepository

        public IMonNodeRepository<TModel, TKey> GetNodeRepository<TModel, TKey>(IMongoCollection<TModel> mongoCollection)
            where TModel : class, IMonNodeModel<TModel, TKey> {
            if (mongoCollection == null)
                throw new ArgumentNullException(nameof(mongoCollection));

            return new MonNodeRepository<TModel, TKey>(this, mongoCollection);
        }

        public IMonNodeRepository<TModel, TKey> GetNodeRepository<TModel, TKey>(string name)
            where TModel : class, IMonNodeModel<TModel, TKey> {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return this.GetNodeRepository<TModel, TKey>(this.MongoDatabase.GetCollection<TModel>(name));
        }

        public IMonNodeRepository<TModel, TKey> GetNodeRepository<TModel, TKey>()
            where TModel : class, IMonNodeModel<TModel, TKey> {
            return this.GetNodeRepository<TModel, TKey>(GetCollectionName<TModel>());
        }

        public IMonNodeRepository<TModel, ObjectId> GetNodeRepository<TModel>(IMongoCollection<TModel> mongoCollection)
            where TModel : class, IMonNodeModel<TModel, ObjectId> {
            if (mongoCollection == null)
                throw new ArgumentNullException(nameof(mongoCollection));

            return new MonNodeRepository<TModel, ObjectId>(this, mongoCollection);
        }

        public IMonNodeRepository<TModel, ObjectId> GetNodeRepository<TModel>(string name)
            where TModel : class, IMonNodeModel<TModel, ObjectId> {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return this.GetNodeRepository<TModel, ObjectId>(this.MongoDatabase.GetCollection<TModel>(name));
        }

        public IMonNodeRepository<TModel, ObjectId> GetNodeRepository<TModel>()
            where TModel : class, IMonNodeModel<TModel, ObjectId> {
            return this.GetNodeRepository<TModel, ObjectId>(GetCollectionName<TModel>());
        }

        #endregion GetNodeRepository

        protected virtual string GetCollectionName<TModel>() {
            var name = MonCollectionAttribute.GetName<TModel>();
            if (name == null)
                name = typeof(TModel).Name;

            return name;
        }

        public IMonGridFSBucket GetBucket(GridFSBucketOptions options) {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            return new MonGridFSBucket(this, new GridFSBucket(this.MongoDatabase, options));
        }

        public IMonGridFSBucket GetBucket() {
            return new MonGridFSBucket(this, new GridFSBucket(this.MongoDatabase));
        }
    }
}
