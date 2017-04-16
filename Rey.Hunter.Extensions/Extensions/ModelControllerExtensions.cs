using Rey.Mon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rey.Mon.Models;
using MongoDB.Bson;

namespace Microsoft.AspNetCore.Mvc {
    public static class ModelControllerExtensions {
        public static IMonServer GetMonServer(this Controller controller) {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.HttpContext.RequestServices.GetService<IMonServer>();
        }

        public static IMonClient GetMonClient(this Controller controller) {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.HttpContext.RequestServices.GetService<IMonClient>();
        }

        public static IMonDatabase GetMonDatabase(this Controller controller) {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.HttpContext.RequestServices.GetService<IMonDatabase>();
        }

        public static IMonCollection<TModel> GetMonCollection<TModel>(this Controller controller, string name) {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return controller.GetMonDatabase().GetCollection<TModel>(name);
        }

        public static IMonCollection<TModel> GetMonCollection<TModel>(this Controller controller) {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.GetMonDatabase().GetCollection<TModel>();
        }

        public static IMonRepository<TModel, TKey> GetMonRepository<TModel, TKey>(this Controller controller)
            where TModel : class, IMonModel<TKey> {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.GetMonDatabase().GetRepository<TModel, TKey>();
        }

        public static IMonRepository<TModel, ObjectId> GetMonRepository<TModel>(this Controller controller)
            where TModel : class, IMonModel<ObjectId> {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.GetMonDatabase().GetRepository<TModel>();
        }

        public static IMonNodeRepository<TModel, TKey> GetMonNodeRepository<TModel, TKey>(this Controller controller)
            where TModel : class, IMonNodeModel<TModel, TKey> {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.GetMonDatabase().GetNodeRepository<TModel, TKey>();
        }

        public static IMonNodeRepository<TModel, ObjectId> GetMonNodeRepository<TModel>(this Controller controller)
            where TModel : class, IMonNodeModel<TModel, ObjectId> {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.GetMonDatabase().GetNodeRepository<TModel>();
        }

        public static IMonGridFSBucket GetBucket(this Controller controller) {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            return controller.GetMonDatabase().GetBucket();
        }
    }
}
