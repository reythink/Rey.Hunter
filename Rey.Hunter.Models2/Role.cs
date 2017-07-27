﻿using Rey.Hunter.Models2.Attributes;

namespace Rey.Hunter.Models2 {
    [MongoCollection("role")]
    public class Role : AccountModel {
        public string Name { get; set; }
        public bool Enabled { get; set; } = true;
    }
}

