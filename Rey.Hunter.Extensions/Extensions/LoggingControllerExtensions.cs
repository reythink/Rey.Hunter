using Microsoft.Extensions.DependencyInjection;
using Rey.Hunter.Models;
using Rey.Hunter.Models.Identity;
using Rey.Hunter.ModelLogging;
using Rey.Mon.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Microsoft.AspNetCore.Mvc {
    public static class ModelLoggingControllerExtensions {
        public static ILogger Logger(this Controller controller) {
            return controller.HttpContext.RequestServices?.GetService<ILogger>();
        }

        public static ILogger Log<TModel, TKey>(this Controller controller, Log<TModel, TKey> log)
            where TModel : class, IMonModel<TKey> {
            return controller.Logger().Log(log);
        }

        public static ILogger Log<TModel, TKey>(this Controller controller, User user, LogAction action, TModel model)
            where TModel : class, IMonModel<TKey> {
            return controller.Log(new Log<TModel, TKey>() { User = user, Action = action, Model = model });
        }

        public static ILogger Log<TModel, TKey>(this Controller controller, LogAction action, TModel model)
            where TModel : class, IMonModel<TKey> {
            return controller.Log<TModel, TKey>(controller.CurrentUser(), action, model);
        }

        public static ILogger Log<TModel, TKey>(this Controller controller, User user, LogAction action, TKey id)
            where TModel : class, IMonModel<TKey> {
            return controller.Log(new Log<TModel, TKey>() { User = user, Action = action, Model = new MonModelRef<TModel, TKey>(id) });
        }

        public static ILogger Log<TModel, TKey>(this Controller controller, LogAction action, TKey id)
            where TModel : class, IMonModel<TKey> {
            return controller.Log<TModel, TKey>(controller.CurrentUser(), action, id);
        }

        public static IEnumerable<Log<TModel, TKey>> Logs<TModel, TKey>(this Controller controller, Expression<Func<Log<TModel, TKey>, bool>> filter)
            where TModel : class, IMonModel<TKey> {
            return controller.Logger().Logs(filter);
        }
    }
}
