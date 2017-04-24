using Rey.Mon.Models;
using Rey.Hunter.Modeling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection {
    public static class JsonModelRefConfigureExtensions {
        public static IServiceCollection AddMvcJsonModelRef(this IServiceCollection services, Action<JsonModelRefOptions> config) {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var options = new JsonModelRefOptions(services);
            config?.Invoke(options);
            return services;
        }

        public class JsonModelRefOptions {
            private IServiceCollection Services { get; }

            public JsonModelRefOptions(IServiceCollection services) {
                this.Services = services;
            }

            public JsonModelRefOptions AddRef<TModel, TKey>()
                where TModel : class, IMonModel<TKey> {
                this.Services.AddSingleton<IConfigureOptions<MvcJsonOptions>, MvcJsonModelRefConfiguration<TModel, TKey>>();
                return this;
            }

            public JsonModelRefOptions AddRef<TModel>()
                where TModel : class, IMonModel<string> {
                this.Services.AddSingleton<IConfigureOptions<MvcJsonOptions>, MvcJsonModelRefConfiguration<TModel, string>>();
                return this;
            }

            public JsonModelRefOptions AddNodeRef<TModel, TKey>()
                where TModel : class, IMonNodeModel<TModel, TKey> {
                this.Services.AddSingleton<IConfigureOptions<MvcJsonOptions>, MvcJsonNodeModelRefConfiguration<TModel, TKey>>();
                return this;
            }

            public JsonModelRefOptions AddNodeRef<TModel>()
               where TModel : class, IMonNodeModel<TModel, string> {
                this.Services.AddSingleton<IConfigureOptions<MvcJsonOptions>, MvcJsonNodeModelRefConfiguration<TModel, string>>();
                return this;
            }
        }
    }
}
