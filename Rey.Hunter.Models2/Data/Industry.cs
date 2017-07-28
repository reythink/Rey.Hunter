using Rey.Hunter.Models2.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rey.Hunter.Models2.Data {
    [MongoCollection("data.industry")]
    public class Industry : AccountModel {
        public string Name { get; set; }
        public bool Root { get; set; }
        public List<string> Children { get; set; } = new List<string>();
    }

    [MongoCollection("data.function")]
    public class Function : AccountModel {
        public string Name { get; set; }
        public bool Root { get; set; }
        public List<string> Children { get; set; } = new List<string>();
    }

    [MongoCollection("data.location")]
    public class Location : AccountModel {
        public string Name { get; set; }
        public bool Root { get; set; }
        public List<string> Children { get; set; } = new List<string>();
    }

    [MongoCollection("data.category")]
    public class Category : AccountModel {
        public string Name { get; set; }
        public bool Root { get; set; }
        public List<string> Children { get; set; } = new List<string>();
    }

    [MongoCollection("data.channel")]
    public class Channel : AccountModel {
        public string Name { get; set; }
        public bool Root { get; set; }
        public List<string> Children { get; set; } = new List<string>();
    }
}
