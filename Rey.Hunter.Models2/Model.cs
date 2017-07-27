using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace Rey.Hunter.Models2 {
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Model : IModel {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
    }
}

