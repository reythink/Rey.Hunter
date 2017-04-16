using Rey.Mon;
using Rey.Mon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rey.MvcExtensions.Modeling.JsonConverters {
    public class JsonModelRefConverter<TModel, TKey> : JsonConverter
        where TModel : class, IMonModel<TKey> {
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
            var reference = value as IMonModelRef<TModel, TKey>;
            var model = reference.Concrete(this.Database);
            serializer.Serialize(writer, model);
        }
    }
}
