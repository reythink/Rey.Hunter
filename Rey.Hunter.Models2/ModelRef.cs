using MongoDB.Bson.Serialization.Attributes;
using Rey.Hunter.Models2.Attributes;
using System.Reflection;

namespace Rey.Hunter.Models2 {
    public class ModelRef<TModel>
        where TModel : class, IModel {
        [BsonElement("$ref")]
        public string Collection { get; set; }

        [BsonElement("$id")]
        public string Key { get; set; }

        [BsonElement("$db")]
        [BsonIgnoreIfNull]
        public string Database { get; set; }

        public ModelRef(string key, string collection, string database) {
            this.Key = key;
            this.Collection = collection;
            this.Database = database;
        }

        public ModelRef(string key, string collection)
            : this(key, collection, null) {
        }

        public ModelRef(string key)
            : this(key, typeof(TModel).GetTypeInfo().GetCustomAttribute<MongoCollectionAttribute>().Collection) {
        }

        public ModelRef(TModel model)
            : this(model.Id) {
        }

        public static implicit operator ModelRef<TModel>(string key) {
            return new ModelRef<TModel>(key);
        }

        public static implicit operator ModelRef<TModel>(TModel model) {
            return new ModelRef<TModel>(model);
        }
    }
}
