using Rey.Hunter.Models2.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rey.Hunter.Models2.Basic {
    [MongoCollection("industry")]
    public class Industry : AccountModel {
        public string Name { get; set; }
        public List<ModelRef<Industry>> Children { get; set; } = new List<ModelRef<Industry>>();
    }
}
