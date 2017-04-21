using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rey.Mon {
    public class MonCollection<TModel> : IMonCollection<TModel> {
        public IMonDatabase Database { get; }
        public IMongoCollection<TModel> MongoCollection { get; }

        public MonCollection(IMonDatabase database, IMongoCollection<TModel> mongoCollection) {
            this.Database = database;
            this.MongoCollection = mongoCollection;
        }

        public void Drop() {
            this.Database.DropCollection(this.MongoCollection.CollectionNamespace.CollectionName);
        }

        public void InsertOne(TModel model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.MongoCollection.InsertOne(model);
        }

        public void InsertMany(IEnumerable<TModel> models) {
            if (models == null)
                throw new ArgumentNullException(nameof(models));

            this.MongoCollection.InsertMany(models);
        }

        public void ReplaceOne(Expression<Func<TModel, bool>> filter, TModel model) {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.MongoCollection.ReplaceOne(filter, model);
        }

        public void UpdateOne(Expression<Func<TModel, bool>> filter, Func<UpdateDefinitionBuilder<TModel>, UpdateDefinition<TModel>> update) {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            if (update == null)
                throw new ArgumentNullException(nameof(update));

            var def = update(Builders<TModel>.Update);
            if (def == null)
                throw new InvalidOperationException("update definition returned is null!");

            this.MongoCollection.UpdateOne(filter, def);
        }

        public void UpdateOne(Expression<Func<TModel, bool>> filter, TModel model, IEnumerable<Expression<Func<TModel, object>>> ignores) {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var ignoreNames = ignores.Select(x => {
                var body = (x.Body as MemberExpression) ?? ((UnaryExpression)x.Body).Operand as MemberExpression;
                return body.Member.Name;
            });
            var type = typeof(TModel).GetTypeInfo();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var builder = Builders<TModel>.Update;
            UpdateDefinition<TModel> definition = null;
            foreach (var property in properties) {
                if (ignoreNames.Contains(property.Name)
                    || property.GetCustomAttribute<BsonIgnoreAttribute>() != null
                    || !property.CanWrite) {
                    continue;
                }

                var value = property.GetValue(model);
                if (definition != null) {
                    definition = definition.Set(property.Name, value);
                } else {
                    definition = builder.Set(property.Name, value);
                }
            }

            if (definition == null)
                return;

            this.MongoCollection.UpdateOne(filter, definition);
        }

        public void UpdateMany(Expression<Func<TModel, bool>> filter, Func<UpdateDefinitionBuilder<TModel>, UpdateDefinition<TModel>> update) {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            if (update == null)
                throw new ArgumentNullException(nameof(update));

            var def = update(Builders<TModel>.Update);
            if (def == null)
                throw new InvalidOperationException("update definition returned is null!");

            this.MongoCollection.UpdateMany(filter, def);
        }

        public void DeleteOne(Expression<Func<TModel, bool>> filter) {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            this.MongoCollection.DeleteOne(filter);
        }

        public void DeleteMany(Expression<Func<TModel, bool>> filter) {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            this.MongoCollection.DeleteMany(filter);
        }

        public IFindFluent<TModel, TModel> Find(Expression<Func<TModel, bool>> filter) {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return this.MongoCollection.Find(filter);
        }

        public TModel FindOne(Expression<Func<TModel, bool>> filter) {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return this.MongoCollection.Find(filter).FirstOrDefault();
        }

        public IEnumerable<TModel> FindMany(Expression<Func<TModel, bool>> filter) {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return this.MongoCollection.Find(filter).ToEnumerable();
        }

        public IMongoQueryable<TModel> Query() {
            return this.MongoCollection.AsQueryable();
        }

        public long Count(Expression<Func<TModel, bool>> filter) {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return this.MongoCollection.Find(filter).Count();
        }

        public bool Exist(Expression<Func<TModel, bool>> filter) {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return Count(filter) > 0;
        }
    }
}
