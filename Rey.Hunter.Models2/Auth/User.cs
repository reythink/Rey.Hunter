using Rey.Hunter.Models2.Attributes;
using System.Collections.Generic;

namespace Rey.Hunter.Models2 {
    [MongoCollection("auth.user")]
    public class User : AccountModel {
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; } = true;
        public string PortraitUrl { get; set; } = "/img/avatar.png";
        public string Position { get; set; }
        public List<RoleRef> Role { get; set; } = new List<RoleRef>();
    }
}

