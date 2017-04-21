using Rey.Mon.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Basic {
    [MonCollection("bus.channels")]
    public class ChannelNode : AccountNodeModel<ChannelNode> {
        public string Name { get; set; }
    }
}
