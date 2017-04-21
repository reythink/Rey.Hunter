using Rey.Identity.Models;
using Rey.Mon.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Rey.Hunter.Models.Identity {
    [MonCollection("ide.roles")]
    public class Role : AccountModel, IRole {
        public string Name { get; set; }
        public bool IsSuper { get; set; }
        public bool Enabled { get; set; } = true;
        public List<AuthItem> AuthItems { get; set; } = new List<AuthItem>();

        public class AuthItem {
            public string Target { get; set; }
            public string Activity { get; set; }
        }
    }
}
