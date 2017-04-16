using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Rey.Mon.Entities {
    public abstract class Entity {
        public override string ToString() {
            return JsonConvert.SerializeObject(this,
                Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}
