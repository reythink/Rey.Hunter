using MongoDB.Driver;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rey.Hunter.Repository {
    public static class ModelRefExtensions {
        public static TModel Concrete<TModel>(this ModelRef<TModel> that, IRepositoryManager manager)
            where TModel : class, IModel {
            var database = that.Database ?? typeof(TModel).GetTypeInfo().GetCustomAttribute<MongoCollectionAttribute>()?.Database ?? manager.DefaultDatabaseName;
            var collection = that.Collection ?? typeof(TModel).GetTypeInfo().GetCustomAttribute<MongoCollectionAttribute>()?.Collection;
            return manager.Client.GetDatabase(database).GetCollection<TModel>(collection).Find(x => x.Id.Equals(that.Key)).SingleOrDefault();
        }
    }
}
