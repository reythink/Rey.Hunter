using MongoDB.Driver;
using System;

namespace Rey.Mon {
    public class MonServer : IMonServer {
        public IMonClient Connect(IMongoClient mongoClient) {
            return new MonClient(this, mongoClient);
        }

        public IMonClient Connect() {
            return Connect(new MongoClient());
        }

        public IMonClient Connect(string conn) {
            if (conn == null)
                throw new ArgumentNullException(nameof(conn));

            return Connect(new MongoClient(conn));
        }

        public IMonClient Connect(MongoClientSettings settings) {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            return Connect(new MongoClient(settings));
        }

        public IMonClient Connect(MongoUrl url) {
            if (url == null)
                throw new ArgumentNullException(nameof(url));

            return Connect(new MongoClient(url));
        }
    }
}
