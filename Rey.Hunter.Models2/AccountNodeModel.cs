using System;

namespace Rey.Hunter.Models2 {
    public abstract class AccountNodeModel : AccountModel, INodeModel {
        public string Name { get; set; }
    }

    public abstract class AccountNodeModel<TModel, TModelRef> : AccountNodeModel, INodeModel<TModel, TModelRef>
        where TModel : class, IAccountModel, INodeModel
        where TModelRef : class, INodeModelRef {
        public TModelRef Parent { get; set; }
        public abstract void SetParent(TModel parent);
    }
}
