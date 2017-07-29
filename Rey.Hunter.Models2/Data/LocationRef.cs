using System;
using System.Collections.Generic;

namespace Rey.Hunter.Models2.Data {
    public class LocationRef : NodeModelRef {
        public List<NodeModelRef> Path { get; set; } = new List<NodeModelRef>();

        public LocationRef(Location model)
            : base(model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Path.AddRange(model.Path);
        }

        public static implicit operator LocationRef(Location model) {
            if (model == null)
                return null;

            return new LocationRef(model);
        }
    }
}
