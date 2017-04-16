using Rey.Mon.Models;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Rey.Hunter.Models.Web.Identity;

namespace Rey.Hunter.Models.Web {
    public class AccountModel : Model {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [BsonIgnoreIfNull]
        public MonStringModelRef<Account> Account { get; set; }
    }
}
