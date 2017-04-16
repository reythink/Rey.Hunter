using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection;

namespace Rey.Mon.JsonConverters {
    public class ObjectIdCreationConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return typeof(ObjectId).GetTypeInfo().IsAssignableFrom(objectType);
        }

        public override bool CanWrite {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            JToken token = JToken.Load(reader);
            if (token == null)
                throw new InvalidOperationException("Token read failed!");

            var id = token.ToObject<string>();
            if (id == null)
                throw new InvalidOperationException("Token cannot convert to string id!");

            return new ObjectId(id);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }
    }
}
