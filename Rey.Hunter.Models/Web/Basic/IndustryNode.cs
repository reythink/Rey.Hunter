using Rey.Mon.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Web.Basic {
    [MonCollection("bus.industries")]
    public class IndustryNode : AccountNodeModel<IndustryNode> {
        public string Name { get; set; }
    }
}
