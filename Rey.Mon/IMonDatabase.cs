using Rey.Mon.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Rey.Mon {
    public interface IMonDatabase {
        IMonClient Client { get; }
        IMongoDatabase MongoDatabase { get; }

        void Drop();

        void CreateCollection(string name);
        void DropCollection(string name);
        void RenameCollection(string oldName, string newName);

        #region GetCollection

        IMonCollection<TModel> GetCollection<TModel>(IMongoCollection<TModel> mongoCollection);
        IMonCollection<TModel> GetCollection<TModel>(string name);
        IMonCollection<TModel> GetCollection<TModel>();

        #endregion GetCollection

        #region GetRepository

        IMonRepository<TModel, TKey> GetRepository<TModel, TKey>(IMongoCollection<TModel> mongoCollection)
            where TModel : class, IMonModel<TKey>;

        IMonRepository<TModel, TKey> GetRepository<TModel, TKey>(string name)
            where TModel : class, IMonModel<TKey>;

        IMonRepository<TModel, TKey> GetRepository<TModel, TKey>()
            where TModel : class, IMonModel<TKey>;

        IMonRepository<TModel, ObjectId> GetRepository<TModel>(IMongoCollection<TModel> mongoCollection)
            where TModel : class, IMonModel<ObjectId>;

        IMonRepository<TModel, ObjectId> GetRepository<TModel>(string name)
            where TModel : class, IMonModel<ObjectId>;

        IMonRepository<TModel, ObjectId> GetRepository<TModel>()
            where TModel : class, IMonModel<ObjectId>;

        #endregion GetRepository

        #region GetNodeRepository

        IMonNodeRepository<TModel, TKey> GetNodeRepository<TModel, TKey>(IMongoCollection<TModel> mongoCollection)
            where TModel : class, IMonNodeModel<TModel, TKey>;

        IMonNodeRepository<TModel, TKey> GetNodeRepository<TModel, TKey>(string name)
            where TModel : class, IMonNodeModel<TModel, TKey>;

        IMonNodeRepository<TModel, TKey> GetNodeRepository<TModel, TKey>()
            where TModel : class, IMonNodeModel<TModel, TKey>;

        IMonNodeRepository<TModel, ObjectId> GetNodeRepository<TModel>(IMongoCollection<TModel> mongoCollection)
            where TModel : class, IMonNodeModel<TModel, ObjectId>;

        IMonNodeRepository<TModel, ObjectId> GetNodeRepository<TModel>(string name)
            where TModel : class, IMonNodeModel<TModel, ObjectId>;

        IMonNodeRepository<TModel, ObjectId> GetNodeRepository<TModel>()
            where TModel : class, IMonNodeModel<TModel, ObjectId>;

        #endregion GetNodeRepository

        IMonGridFSBucket GetBucket(GridFSBucketOptions options);
        IMonGridFSBucket GetBucket();
    }
}
