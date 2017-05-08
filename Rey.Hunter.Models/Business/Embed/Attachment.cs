using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models.Business {
    public class Attachment {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ContentType { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreateAt { get; set; } = DateTime.Now;
    }
}
