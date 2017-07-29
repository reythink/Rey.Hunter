using System;

namespace Rey.Hunter.Models2.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoCollectionAttribute : Attribute {
        public string Database { get; }
        public string Collection { get; }

        public MongoCollectionAttribute(string collection, string database) {
            this.Collection = collection;
            this.Database = database;
        }

        public MongoCollectionAttribute(string collection)
            : this(collection, null) {
        }
    }
}
