using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Rey.Hunter.Models2 {
    public abstract class ModelRef : IModelRef {
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? UpdateAt { get; set; }
    }

    public abstract class ModelRef<TModel> : ModelRef
        where TModel : class, IModel {
    }
}
