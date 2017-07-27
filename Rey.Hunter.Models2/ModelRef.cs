using MongoDB.Bson.Serialization.Attributes;
using Rey.Hunter.Models2.Attributes;
using System.Reflection;

namespace Rey.Hunter.Models2 {
    [BsonIgnoreExtraElements(Inherited = true)]
    public class ModelRef<TModel>
        where TModel : class, IModel {
        public string Id { get; set; }

        public ModelRef(string id) {
            this.Id = id;
        }

        public ModelRef(TModel model)
            : this(model.Id) {
        }

        public static implicit operator ModelRef<TModel>(string id) {
            return new ModelRef<TModel>(id);
        }

        public static implicit operator ModelRef<TModel>(TModel model) {
            return new ModelRef<TModel>(model);
        }
    }
}
