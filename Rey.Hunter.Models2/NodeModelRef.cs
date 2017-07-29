using System;

namespace Rey.Hunter.Models2 {
    public class NodeModelRef : ModelRef, INodeModelRef {
        public string Name { get; set; }

        public NodeModelRef(AccountNodeModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Id = model.Id;
            this.Name = model.Name;
        }

        public static implicit operator NodeModelRef(AccountNodeModel model) {
            if (model == null)
                return null;

            return new NodeModelRef(model);
        }
    }
}
