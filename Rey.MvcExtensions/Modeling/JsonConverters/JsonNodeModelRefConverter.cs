using Rey.Mon;
using Rey.Mon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rey.MvcExtensions.Modeling.JsonConverters {
    public class JsonNodeModelRefConverter<TModel, TKey> : JsonConverter
        where TModel : class, IMonNodeModel<TModel, TKey> {
        private IMonDatabase Database { get; }

        public override bool CanRead {
            get { return false; }
        }

        public JsonNodeModelRefConverter(IMonDatabase database) {
            this.Database = database;
        }

        public override bool CanConvert(Type objectType) {
            var type = typeof(IMonNodeModelRef<TModel, TKey>);
            return type.GetTypeInfo().IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var reference = value as IMonNodeModelRef<TModel, TKey>;
            var model = reference.Concrete(this.Database);
            serializer.Serialize(writer, model);
        }
    }
}
