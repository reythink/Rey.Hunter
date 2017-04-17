using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models.Web;
using Rey.Mon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Rey.Hunter {
    public class QueryBuilder<TModel> {
        public IMonCollection<TModel> Collection { get; }

        private List<FilterDefinition<TModel>> Filters { get; } = new List<FilterDefinition<TModel>>();

        public QueryBuilder(IMonCollection<TModel> collection) {
            this.Collection = collection;
        }

        public QueryBuilder<TModel> AddFilter(FilterDefinition<TModel> filter) {
            this.Filters.Add(filter);
            return this;
        }

        public QueryBuilder<TModel> AddInFilter<TItem>(Expression<Func<TModel, TItem>> field, IEnumerable<TItem> values) {
            var filters = new List<FilterDefinition<TModel>>();

            foreach (var value in values) {
                filters.Add(Builders<TModel>.Filter.Eq(field, value));
            }

            if (filters.Count > 0) {
                this.Filters.Add(Builders<TModel>.Filter.Or(filters));
            }

            return this;
        }

        public QueryBuilder<TModel> AddEnumInFilter<TItem>(Expression<Func<TModel, TItem>> field, IEnumerable<string> values) {
            var type = typeof(TItem);
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                type = type.GetGenericArguments()[0];
            }

            if (!type.GetTypeInfo().IsEnum)
                throw new InvalidCastException("TItem isn't a type of enum!");

            return AddInFilter(field, values.Select(x => (TItem)Enum.Parse(type, x)));
        }

        public QueryBuilder<TModel> AddStringInFilter(Expression<Func<TModel, object>> field, IEnumerable<string> values, bool ignoreCase = false) {
            var filters = new List<FilterDefinition<TModel>>();

            foreach (var value in values) {
                filters.Add(Builders<TModel>.Filter.Regex(field, new BsonRegularExpression(value, ignoreCase ? "i" : "")));
            }

            if (filters.Count > 0) {
                this.Filters.Add(Builders<TModel>.Filter.Or(filters));
            }

            return this;
        }

        public QueryBuilder<TModel> AddStringInFilter<TItem>(Expression<Func<TModel, IEnumerable<TItem>>> field, Expression<Func<TItem, object>> item, IEnumerable<string> values, bool ignoreCase = false) {
            var filters = new List<FilterDefinition<TModel>>();

            foreach (var value in values) {
                filters.Add(Builders<TModel>.Filter.ElemMatch(field, Builders<TItem>.Filter.Regex(item, new BsonRegularExpression(value, ignoreCase ? "i" : ""))));
            }

            if (filters.Count > 0) {
                this.Filters.Add(Builders<TModel>.Filter.Or(filters));
            }

            return this;
        }

        public QueryBuilder<TModel> AddSearchFilter(string search, params Expression<Func<TModel, object>>[] fields) {
            if (string.IsNullOrEmpty(search))
                return this;

            var filters = new List<FilterDefinition<TModel>>();

            foreach (var field in fields) {
                filters.Add(Builders<TModel>.Filter.Regex(field, new BsonRegularExpression(search, "i")));
            }

            if (filters.Count > 0) {
                this.Filters.Add(Builders<TModel>.Filter.Or(filters));
            }

            return this;
        }

        public FilterDefinition<TModel> BuildFilter() {
            return Builders<TModel>.Filter.And(this.Filters);
        }

        public IQueryable<TModel> Build() {
            var filter = this.BuildFilter();
            return this.Collection.MongoCollection.Find(filter).ToList().AsQueryable();
        }
    }

    public static class QueryBuilderExtensions {
        public static QueryBuilder<TModel> AddAccountFilter<TModel>(this QueryBuilder<TModel> builder, string id)
            where TModel : AccountModel {
            return builder.AddFilter(Builders<TModel>.Filter.Eq(x => x.Account.Id, id));
        }
    }
}
