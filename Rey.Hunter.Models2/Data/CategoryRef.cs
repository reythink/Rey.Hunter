using System;
using System.Collections.Generic;

namespace Rey.Hunter.Models2.Data {
    public class CategoryRef : NodeModelRef {
        public List<NodeModelRef> Path { get; set; } = new List<NodeModelRef>();

        public CategoryRef(Category model)
            : base(model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Path.AddRange(model.Path);
        }

        public static implicit operator CategoryRef(Category model) {
            if (model == null)
                return null;

            return new CategoryRef(model);
        }
    }
}
