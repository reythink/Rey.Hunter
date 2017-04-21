using Rey.Mon.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Basic {
    [MonCollection("bus.categories")]
    public class CategoryNode : AccountNodeModel<CategoryNode> {
        public string Name { get; set; }
    }
}
