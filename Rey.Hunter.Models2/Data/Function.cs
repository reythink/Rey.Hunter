using System;
using Rey.Hunter.Models2.Attributes;

namespace Rey.Hunter.Models2.Data {
    [MongoCollection("data.function")]
    public class Function : AccountNodeModel<Function, FunctionRef> {
        public override void SetParent(Function parent) {
            this.Parent = new FunctionRef(parent);
        }
    }
}
