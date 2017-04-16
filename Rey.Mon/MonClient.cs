using Rey.Mon.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rey.Mon {
    public class MonClient : IMonClient {
        public IMonServer Server { get; }
        public IMongoClient MongoClient { get; }

        public MonClient(IMonServer server, IMongoClient mongoClient) {
            this.Server = server;
            this.MongoClient = mongoClient;
        }

        public IEnumerable<IDatabaseDesc> GetDatabaseDescs() {
            return this.MongoClient.ListDatabases()
                .ToEnumerable()
                .Select(x => new DatabaseDesc() {
                    Name = x.GetValue("name").AsString,
                    SizeOnDisk = x.GetValue("sizeOnDisk").AsDouble,
                    Empty = x.GetValue("empty").AsBoolean
                });
        }

        public IDatabaseDesc GetDatabaseDesc(string name) {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return this.GetDatabaseDescs().FirstOrDefault(x => x.Name.Equals(name));
        }

        public IMonDatabase GetDatabase(IMongoDatabase mongoDatabase) {
            if (mongoDatabase == null)
                throw new ArgumentNullException(nameof(mongoDatabase));

            return new MonDatabase(this, mongoDatabase);
        }

        public IMonDatabase GetDatabase(string name) {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return this.GetDatabase(this.MongoClient.GetDatabase(name));
        }

        public void DropDatabase(string name) {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.MongoClient.DropDatabase(name);
        }
    }
}
