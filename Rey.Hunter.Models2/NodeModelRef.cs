using System;

namespace Rey.Hunter.Models2 {
    public abstract class NodeModelRef : ModelRef, INodeModelRef {
        public string Name { get; set; }
    }

    public class NodeModelRef<TModel> : ModelRef<TModel>, INodeModelRef
        where TModel : class, INodeModel {
        public string Name { get; set; }

        public NodeModelRef(TModel model) {
            this.Id = model.Id;
            this.Name = model.Name;
        }
    }
}
