using System;
using System.Collections.Generic;

namespace Rey.Hunter.Models2.Data {
    public class ChannelRef : NodeModelRef {
        public List<NodeModelRef> Path { get; set; } = new List<NodeModelRef>();

        public ChannelRef(Channel model)
            : base(model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Path.AddRange(model.Path);
        }

        public static implicit operator ChannelRef(Channel model) {
            if (model == null)
                return null;

            return new ChannelRef(model);
        }
    }
}
