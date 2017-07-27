using Rey.Hunter.Models2.Attributes;

namespace Rey.Hunter.Models2 {
    [MongoCollection("account")]
    public class Account : Model {
        public string Company { get; set; }
        public bool Enabled { get; set; } = true;
    }
}

