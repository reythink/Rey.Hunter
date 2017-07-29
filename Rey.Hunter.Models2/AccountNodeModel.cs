using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rey.Hunter.Models2 {
    public abstract class AccountNodeModel : AccountModel {
        public string Name { get; set; }
        public NodeModelRef Parent { get; private set; }
        public List<NodeModelRef> Path { get; set; } = new List<NodeModelRef>();

        public AccountNodeModel SetParent(AccountNodeModel model) {
            this.Parent = model;
            this.Path.AddRange(model.Path);
            this.Path.Add(model);
            return this;
        }
    }

    public class NodeModelRef : ModelRef {
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
