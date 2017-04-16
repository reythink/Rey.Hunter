using Rey.Mon.Entities;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Rey.Mon {
    public interface IMonClient {
        IMonServer Server { get; }
        IMongoClient MongoClient { get; }

        IEnumerable<IDatabaseDesc> GetDatabaseDescs();
        IDatabaseDesc GetDatabaseDesc(string name);

        IMonDatabase GetDatabase(IMongoDatabase mongoDatabase);
        IMonDatabase GetDatabase(string name);

        void DropDatabase(string name);
    }
}
