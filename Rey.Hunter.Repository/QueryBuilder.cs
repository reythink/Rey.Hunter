using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Rey.Hunter.Repository {
    public abstract class QueryBuilder<TModel> : IQueryBuilder<TModel> {
        protected IRepository<TModel> Repository { get; }
        public QueryBuilder(IRepository<TModel> repository) {
            this.Repository = repository;
        }

        public abstract IEnumerable<TModel> ToEnumerable();

        //public IQueryBuilder<TModel> Aggregate(Func<IAggregateFluent<BsonDocument>, IAggregateFluent<BsonDocument>> build) {
        //    this.Agg = build.Invoke(this.Agg);
        //    return this;
        //}

        //public IQueryBuilder<TModel> Filter(Func<FilterDefinitionBuilder<BsonDocument>, FilterDefinition<BsonDocument>> build) {
        //    var builder = Builders<BsonDocument>.Filter;
        //    var filter = build.Invoke(builder);
        //    this.Filters.Add(filter);
        //    return this;
        //}

        //public virtual IEnumerable<TModel> ToEnumerable() {
        //    var filter = Builders<BsonDocument>.Filter.And(this.Filters);
        //    this.Agg = this.Agg.Match(filter);
        //    var proj = Builders<BsonDocument>.Projection.As<TModel>();
        //    return this.Agg.Project(proj).ToEnumerable();
        //}
    }
}
