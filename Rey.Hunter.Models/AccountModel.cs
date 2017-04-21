using Rey.Mon.Models;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Rey.Hunter.Models.Identity;

namespace Rey.Hunter.Models {
    public class AccountModel : Model {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [BsonIgnoreIfNull]
        public MonStringModelRef<Account> Account { get; set; }
    }
}
