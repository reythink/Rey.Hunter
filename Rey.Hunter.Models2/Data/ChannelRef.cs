using System;

namespace Rey.Hunter.Models2.Data {
    public class ChannelRef : ModelRef {
        public string Name { get; set; }

        public ChannelRef(Channel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Id = model.Id;
            this.Name = model.Name;
        }

        public static implicit operator ChannelRef(Channel model) {
            if (model == null)
                return null;

            return new ChannelRef(model);
        }
    }
}
