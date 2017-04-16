using Rey.Mon.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Web.Basic {
    [MonCollection("bas.locations")]
    public class LocationNode : AccountNodeModel<LocationNode> {
        public string Name { get; set; }
    }
}
