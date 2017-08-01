using System;
using Rey.Hunter.Models2.Attributes;

namespace Rey.Hunter.Models2.Data {
    [MongoCollection("data.category")]
    public class Category : AccountNodeModel<Category, CategoryRef> {
        public override void SetParent(Category parent) {
            this.Parent = new CategoryRef(parent);
        }
    }
}
