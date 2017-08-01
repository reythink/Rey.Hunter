namespace Rey.Hunter.Models2 {
    public interface INodeModel : IModel {
        string Name { get; }
    }

    public interface INodeModel<TModel, TModelRef> : INodeModel
        where TModel : class, INodeModel
        where TModelRef : class, INodeModelRef {
        TModelRef Parent { get; }
        void SetParent(TModel parent);
    }
}
