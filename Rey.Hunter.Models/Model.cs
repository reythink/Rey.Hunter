using Rey.Mon.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Dynamic;

namespace Rey.Hunter.Models {
    public abstract class Model : MonStringModel {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreateAt { get; set; } = DateTime.Now;
    }
}
