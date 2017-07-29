using System;
using System.Collections.Generic;

namespace Rey.Hunter.Models2.Data {
    public class IndustryRef : NodeModelRef {
        public List<NodeModelRef> Path { get; set; } = new List<NodeModelRef>();

        public IndustryRef(Industry model)
            : base(model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Path.AddRange(model.Path);
        }

        public static implicit operator IndustryRef(Industry model) {
            if (model == null)
                return null;

            return new IndustryRef(model);
        }
    }
}
