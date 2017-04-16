using Rey.Mon.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Dynamic;

namespace Rey.Hunter.Models {
    public abstract class Model : MonStringModel {
        [BsonIgnore]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreateAt {
            get {
                if (string.IsNullOrEmpty(this.Id)) { return null; }
                try {
                    return new ObjectId(this.Id).CreationTime;
                } catch (Exception) {
                    return null;
                }
            }
        }
    }
}
