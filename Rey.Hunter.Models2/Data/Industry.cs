using System;
using Rey.Hunter.Models2.Attributes;

namespace Rey.Hunter.Models2.Data {
    [MongoCollection("data.industry")]
    public class Industry : AccountNodeModel<Industry, IndustryRef> {
        public override void SetParent(Industry parent) {
            this.Parent = new IndustryRef(parent);
        }
    }
}
