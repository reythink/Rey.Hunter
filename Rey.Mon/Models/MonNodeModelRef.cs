using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Mon.Models {
    public class MonNodeModelRef<TModel, TKey> : IMonNodeModelRef<TModel, TKey>
        where TModel : class, IMonNodeModel<TModel, TKey> {
        public TKey Id { get; set; }

        public MonNodeModelRef() {
        }

        public MonNodeModelRef(TModel model) {
            this.Id = model.Id;
        }

        public TModel Concrete(IMonNodeRepository<TModel, TKey> repository) {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            return repository.FindNode(this.Id);
        }

        public TModel Concrete(IMonDatabase database) {
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            return Concrete(database.GetNodeRepository<TModel, TKey>());
        }

        public static implicit operator MonNodeModelRef<TModel, TKey>(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (object.Equals(model.Id, default(TKey)))
                throw new InvalidOperationException("Id is empty!");

            return new MonNodeModelRef<TModel, TKey>(model);
        }
    }

    public class MonNodeModelRef<TModel> : MonNodeModelRef<TModel, ObjectId>
        where TModel : class, IMonNodeModel<TModel, ObjectId> {
        public MonNodeModelRef()
            : base() {
        }

        public MonNodeModelRef(TModel model)
            : base(model) {
        }

        public static implicit operator MonNodeModelRef<TModel>(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IsIdEmpty())
                throw new InvalidOperationException("Id is empty!");

            return new MonNodeModelRef<TModel>(model);
        }
    }

    public class MonStringNodeModelRef<TModel> : MonNodeModelRef<TModel, string>
        where TModel : class, IMonNodeModel<TModel, string> {
        public MonStringNodeModelRef()
            : base() {
        }

        public MonStringNodeModelRef(TModel model)
            : base(model) {
        }

        public static implicit operator MonStringNodeModelRef<TModel>(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IsIdEmpty())
                throw new InvalidOperationException("Id is empty!");

            return new MonStringNodeModelRef<TModel>(model);
        }
    }

    public class MonGuidNodeModelRef<TModel> : MonNodeModelRef<TModel, Guid>
        where TModel : class, IMonNodeModel<TModel, Guid> {
        public MonGuidNodeModelRef()
            : base() {
        }

        public MonGuidNodeModelRef(TModel model)
            : base(model) {
        }

        public static implicit operator MonGuidNodeModelRef<TModel>(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IsIdEmpty())
                throw new InvalidOperationException("Id is empty!");

            return new MonGuidNodeModelRef<TModel>(model);
        }
    }
}
