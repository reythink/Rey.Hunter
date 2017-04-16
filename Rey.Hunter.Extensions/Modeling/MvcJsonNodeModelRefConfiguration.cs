using Rey.Mon;
using Rey.Mon.Models;
using Rey.Hunter.Modeling.JsonConverters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Modeling {
    public class MvcJsonNodeModelRefConfiguration<TModel, TKey> : IConfigureOptions<MvcJsonOptions>
        where TModel : class, IMonNodeModel<TModel, TKey> {
        private IMonDatabase Database { get; }

        public MvcJsonNodeModelRefConfiguration(IMonDatabase database) {
            this.Database = database;
        }

        public void Configure(MvcJsonOptions options) {
            options.SerializerSettings.Converters.Add(new JsonNodeModelRefConverter<TModel, TKey>(this.Database));
        }
    }
}
