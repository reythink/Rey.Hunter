using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace Rey.Mon.Models {
    public abstract class MonGuidModel : IMonModel<Guid> {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }

        public bool IsIdEmpty() {
            return this.Id == Guid.Empty;
        }

        public virtual Guid GenerateId() {
            return (Guid)GuidGenerator.Instance.GenerateId(this.GetType(), this);
        }
    }
}
