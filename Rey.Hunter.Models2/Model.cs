using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace Rey.Hunter.Models2 {
    public class Model {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreateAt { get; set; } = DateTime.Now;
    }

    public class Account : Model {
        public string Company { get; set; }
        public bool Enabled { get; set; } = true;
    }

    public class Role : AccountModel {
        public string Name { get; set; }
        public bool Enabled { get; set; } = true;
    }

    public class User : AccountModel {
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; } = true;
        public string PortraitUrl { get; set; } = "/img/avatar.png";
        public string Position { get; set; }
    }

    public class AccountModel {
        public Account Account { get; set; }
    }
}

