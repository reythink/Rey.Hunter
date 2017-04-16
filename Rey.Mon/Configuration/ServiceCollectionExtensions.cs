using Rey.Mon;
using Rey.Mon.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddReyMon(this IServiceCollection services, MonOptions options) {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var server = new MonServer();
            services.AddSingleton<IMonServer>(server);

            var client = string.IsNullOrEmpty(options.ConnectionString) ? server.Connect() : server.Connect(options.ConnectionString);
            services.AddSingleton<IMonClient>(client);

            if (!string.IsNullOrEmpty(options.DefaultDatabaseName)) {
                var database = client.GetDatabase(options.DefaultDatabaseName);
                services.AddSingleton<IMonDatabase>(database);
            }

            return services;
        }

        public static IServiceCollection AddReyMon(this IServiceCollection services, Action<MonOptions> setOptions = null) {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var options = new MonOptions();
            setOptions?.Invoke(options);
            return AddReyMon(services, options);
        }
    }
}
