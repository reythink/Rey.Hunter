using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Rey.Mon.Models {
    public abstract class MonModel : IMonModel<ObjectId> {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }

        public bool IsIdEmpty() {
            return this.Id == ObjectId.Empty;
        }

        public virtual ObjectId GenerateId() {
            return (ObjectId)ObjectIdGenerator.Instance.GenerateId(this.GetType(), this);
        }
    }
}
