using MongoDB.Bson.Serialization.Attributes;

namespace Rey.Hunter.Models2.Business {
    public class TalentContact {
        [BsonIgnoreIfNull]
        public string Phone { get; set; }

        [BsonIgnoreIfNull]
        public string Mobile { get; set; }

        [BsonIgnoreIfNull]
        public string Email { get; set; }

        [BsonIgnoreIfNull]
        public string QQ { get; set; }

        [BsonIgnoreIfNull]
        public string Wechat { get; set; }
    }
}
