using Rey.Mon.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Basic {
    [MonCollection("bas.functions")]
    public class FunctionNode : AccountNodeModel<FunctionNode> {
        public string Name { get; set; }
    }
}
