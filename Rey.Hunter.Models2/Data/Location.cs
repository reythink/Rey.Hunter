using System;
using Rey.Hunter.Models2.Attributes;

namespace Rey.Hunter.Models2.Data {
    [MongoCollection("data.location")]
    public class Location : AccountNodeModel<Location, LocationRef> {
        public override void SetParent(Location parent) {
            this.Parent = new LocationRef(parent);
        }
    }
}
