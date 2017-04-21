using Rey.Mon.Models;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Rey.Hunter.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Models {
    public class AccountNodeModel<TModel> : NodeModel<TModel>
        where TModel : class, IMonNodeModel<TModel, string> {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [BsonIgnoreIfNull]
        public MonStringModelRef<Account> Account { get; set; }
    }
}
