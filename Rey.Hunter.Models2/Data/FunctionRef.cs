using System;
using System.Collections.Generic;

namespace Rey.Hunter.Models2.Data {
    public class FunctionRef : NodeModelRef {
        public List<NodeModelRef> Path { get; set; } = new List<NodeModelRef>();

        public FunctionRef(Function model)
            : base(model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Path.AddRange(model.Path);
        }

        public static implicit operator FunctionRef(Function model) {
            if (model == null)
                return null;

            return new FunctionRef(model);
        }
    }
}
