using MongoDB.Bson.Serialization.Attributes;

namespace Rey.Hunter.Models2.Business {
    public class TalentContact {
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        [BsonElement("QQ")]
        public string QQ { get; set; }

        public string Wechat { get; set; }
    }
}
