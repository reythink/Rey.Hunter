using MongoDB.Driver;

namespace Rey.Mon {
    public interface IMonServer {
        IMonClient Connect(IMongoClient mongoClient);
        IMonClient Connect();
        IMonClient Connect(string conn);
        IMonClient Connect(MongoClientSettings settings);
        IMonClient Connect(MongoUrl url);
    }
}
