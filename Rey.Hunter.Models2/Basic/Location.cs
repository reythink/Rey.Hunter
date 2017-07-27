using Rey.Hunter.Models2.Attributes;

namespace Rey.Hunter.Models2.Basic {
    [MongoCollection("location")]
    public class Location : AccountModel {
        public string Name { get; set; }
    }
}
