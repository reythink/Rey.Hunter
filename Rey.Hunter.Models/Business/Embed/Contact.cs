using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Business {
    public class Contact {
        [BsonIgnoreIfNull]
        public string Name { get; set; }

        [BsonIgnoreIfNull]
        public string Title { get; set; }

        [BsonIgnoreIfNull]
        public string Phone { get; set; }

        [BsonIgnoreIfNull]
        public string Mobile { get; set; }

        [BsonIgnoreIfNull]
        public string Wechat { get; set; }

        [BsonIgnoreIfNull]
        public string Email { get; set; }

        [BsonIgnoreIfNull]
        public string Address { get; set; }

        [BsonIgnoreIfNull]
        public string Notes { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? Birthday { get; set; }

        [BsonIgnoreIfNull]
        public string QQ { get; set; }
    }
}
