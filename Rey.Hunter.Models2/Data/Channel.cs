using System;
using Rey.Hunter.Models2.Attributes;

namespace Rey.Hunter.Models2.Data {
    [MongoCollection("data.channel")]
    public class Channel : AccountNodeModel<Channel, ChannelRef> {
        public override void SetParent(Channel parent) {
            this.Parent = new ChannelRef(parent);
        }
    }
}
