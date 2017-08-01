using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Rey.Hunter.Models2 {
    public abstract class ModelRef : IModelRef {
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? UpdateAt { get; set; }
    }

    public abstract class ModelRef<TModel> : ModelRef, IModelRef<TModel>
        where TModel : class, IModel {
        public ModelRef(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Init(model);
        }

        public virtual void Init(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Id = model.Id;
        }

        public virtual void Update(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Init(model);
            this.UpdateAt = DateTime.Now;
        }
    }
}
