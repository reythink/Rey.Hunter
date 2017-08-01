using System;

namespace Rey.Hunter.Models2 {
    public abstract class NodeModelRef : ModelRef, INodeModelRef {
        public string Name { get; set; }
    }

    public class NodeModelRef<TModel> : ModelRef<TModel>, INodeModelRef
        where TModel : class, INodeModel {
        public string Name { get; set; }

        public NodeModelRef(TModel model) 
            : base(model) {
        }

        public override void Init(TModel model) {
            base.Init(model);
            this.Name = model.Name;
        }
    }
}
