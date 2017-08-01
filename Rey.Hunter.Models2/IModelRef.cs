using System;

namespace Rey.Hunter.Models2 {
    public interface IModelRef {
        string Id { get; }
        DateTime? UpdateAt { get; }
    }

    public interface IModelRef<TModel> : IModelRef
        where TModel : class, IModel {
        void Init(TModel model);
        void Update(TModel model);
    }
}
