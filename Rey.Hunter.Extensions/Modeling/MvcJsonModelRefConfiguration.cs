using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Rey.Mon.Models;
using Rey.Mon;
using Rey.Hunter.Modeling.JsonConverters;

namespace Rey.Hunter.Modeling {
    public class MvcJsonModelRefConfiguration<TModel, TKey> : IConfigureOptions<MvcJsonOptions>
        where TModel : class, IMonModel<TKey> {
        private IMonDatabase Database { get; }

        public MvcJsonModelRefConfiguration(IMonDatabase database) {
            this.Database = database;
        }

        public void Configure(MvcJsonOptions options) {
            options.SerializerSettings.Converters.Add(new JsonModelRefConverter<TModel, TKey>(this.Database));
        }
    }
}
