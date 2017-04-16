using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Rey.Mon.Models {
    public abstract class MonStringModel : IMonModel<string> {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        public bool IsIdEmpty() {
            return string.IsNullOrEmpty(this.Id);
        }

        public virtual string GenerateId() {
            return (string)StringObjectIdGenerator.Instance.GenerateId(this.GetType(), this);
        }
    }
}
