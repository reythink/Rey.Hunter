using MongoDB.Bson;
using System;

namespace Rey.Mon.Models {
    public class MonModelRef<TModel, TKey> : IMonModelRef<TModel, TKey>
        where TModel : class, IMonModel<TKey> {
        public TKey Id { get; set; }

        public MonModelRef() {
        }

        public MonModelRef(TModel model) {
            this.Id = model.Id;
        }

        public TModel Concrete(IMonCollection<TModel> collection) {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return collection.FindOne(x => x.Id.Equals(this.Id));
        }

        public TModel Concrete(IMonDatabase database) {
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            return Concrete(database.GetCollection<TModel>());
        }

        public static implicit operator MonModelRef<TModel, TKey>(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (object.Equals(model.Id, default(TKey)))
                throw new InvalidOperationException("Id is empty!");

            return new MonModelRef<TModel, TKey>(model);
        }
    }

    public class MonModelRef<TModel> : MonModelRef<TModel, ObjectId>
        where TModel : MonModel {
        public MonModelRef()
            : base() {
        }

        public MonModelRef(TModel model)
            : base(model) {
        }

        public static implicit operator MonModelRef<TModel>(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IsIdEmpty())
                throw new InvalidOperationException("Id is empty!");

            return new MonModelRef<TModel>(model);
        }
    }
}
