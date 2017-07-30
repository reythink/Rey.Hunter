using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace Rey.Hunter.Models2 {
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class Model : IModel {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreateAt { get; set; } = DateTime.Now;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ModifyAt { get; set; }

        public void UpdateModifyAt() {
            this.ModifyAt = DateTime.Now;
        }
    }
}

