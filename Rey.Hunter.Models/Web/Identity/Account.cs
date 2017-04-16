using Rey.Identity.Models;
using Rey.Mon.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Rey.Hunter.Models.Web.Identity {
    [MonCollection("ide.accounts")]
    public class Account : Model, IAccount {
        public string Company { get; set; }
        public bool Enabled { get; set; } = true;
    }
}
