using Rey.Mon;
using Rey.Mon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Rey.Hunter.Modeling.JsonConverters {
    public class JsonModelRefConverter<TModel, TKey> : JsonConverter
        where TModel : class, IMonModel<TKey> {
        private static List<Type> TypeStacks { get; } = new List<Type>();
        private IMonDatabase Database { get; }

        public override bool CanRead {
            get { return false; }
        }

        public JsonModelRefConverter(IMonDatabase database) {
            this.Database = database;
        }

        public override bool CanConvert(Type objectType) {
            var type = typeof(IMonModelRef<TModel, TKey>);
            return type.GetTypeInfo().IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var type = typeof(TModel);
            if (TypeStacks.Contains(type)) {
                JObject.FromObject(value).WriteTo(writer);
                TypeStacks.Clear();
                return;
            }

            var reference = value as IMonModelRef<TModel, TKey>;
            var model = reference.Concrete(this.Database);
            TypeStacks.Add(type);

            serializer.Serialize(writer, model);
            TypeStacks.Clear();
        }
    }
}
