using MongoDB.Bson.Serialization.Attributes;

namespace Rey.Hunter.Models2 {
    public class ModelRef<TModel>
        where TModel : class, IModel {
        public string Id { get; set; }

        [BsonIgnore]
        public TModel Model { get; set; }

        public ModelRef(string id) {
            this.Id = id;
        }

        public ModelRef(TModel model) {
            this.Id = model.Id;
            this.Model = model;
        }

        public static implicit operator ModelRef<TModel>(string id) {
            return new ModelRef<TModel>(id);
        }

        public static implicit operator ModelRef<TModel>(TModel model) {
            return new ModelRef<TModel>(model);
        }
    }

    public interface IAccountModel : IModel {
        ModelRef<Account> Account { get; set; }
    }

    public abstract class AccountModel : Model, IAccountModel {
        public ModelRef<Account> Account { get; set; }
    }
}
