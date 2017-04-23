using Rey.Hunter.ModelLogging;
using Rey.Hunter.ModelLogging.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ModelLoggingServiceCollectionExtensions {
        public static IServiceCollection AddModelLogging(this IServiceCollection services, ModelLoggingOptions options) {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            services.AddSingleton(options);
            services.AddSingleton<ILogger, Logger>();
            return services;
        }

        public static IServiceCollection AddModelLogging(this IServiceCollection services, Action<ModelLoggingOptions> config = null) {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var options = new ModelLoggingOptions();
            config?.Invoke(options);
            return AddModelLogging(services, options);
        }
    }
}
