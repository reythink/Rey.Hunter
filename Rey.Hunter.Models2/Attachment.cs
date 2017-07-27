using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Rey.Hunter.Models2 {
    public class Attachment {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ContentType { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreateAt { get; set; } = DateTime.Now;
    }
}
