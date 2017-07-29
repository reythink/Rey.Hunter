using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rey.Hunter.Repository {
    public abstract class QueryBuilder<TModel> : IQueryBuilder<TModel> {
        protected IRepository<TModel> Repository { get; }

        protected static FilterDefinitionBuilder<TModel> FilterBuilder => Builders<TModel>.Filter;
        protected static SortDefinitionBuilder<TModel> SortBuilder => Builders<TModel>.Sort;

        public QueryBuilder(IRepository<TModel> repository) {
            this.Repository = repository;
        }

        protected abstract FilterDefinition<TModel> BuildFilter();
        protected abstract SortDefinition<TModel> BuildSort();
        protected abstract int? BuildSkip();
        protected abstract int? BuildLimit();

        public virtual IEnumerable<TModel> Build(Action<QueryResult> result = null) {
            var fluent = this.Repository
                .Collection
                .Find(this.BuildFilter());

            result?.Invoke(new QueryResult(fluent.Count()));

            return fluent
                .Sort(this.BuildSort())
                .Skip(this.BuildSkip())
                .Limit(this.BuildLimit())
                .ToEnumerable();
        }
    }

    public abstract class QueryBuilder<TModel, TBuilder> : QueryBuilder<TModel>, IQueryBuilder<TModel, TBuilder>
       where TBuilder : class, IQueryBuilder<TModel> {
        private Dictionary<string, List<FilterDefinition<TModel>>> Filters { get; } = new Dictionary<string, List<FilterDefinition<TModel>>>();
        private List<SortDefinition<TModel>> Sorts { get; } = new List<SortDefinition<TModel>>();
        private int? Skip { get; set; }
        private int? Limit { get; set; }

        public QueryBuilder(IRepository<TModel> repository)
            : base(repository) {
        }

        private List<FilterDefinition<TModel>> GetNameFilters(string name) {
            if (!this.Filters.ContainsKey(name)) {
                return this.Filters[name] = new List<FilterDefinition<TModel>>();
            }
            return this.Filters[name];
        }

        public virtual TBuilder AddFilter(string name, FilterDefinition<TModel> filter) {
            this.GetNameFilters(name).Add(filter);
            return this as TBuilder;
        }

        public virtual TBuilder AddFilters(string name, IEnumerable<FilterDefinition<TModel>> filters) {
            this.GetNameFilters(name).AddRange(filters);
            return this as TBuilder;
        }

        protected override FilterDefinition<TModel> BuildFilter() {
            if (this.Filters.Count == 0)
                return FilterBuilder.Empty;

            var filters = new List<FilterDefinition<TModel>>();
            foreach (var key in this.Filters.Keys) {
                filters.Add(FilterBuilder.Or(this.Filters[key]));
            }
            return FilterBuilder.And(filters);
        }

        protected override SortDefinition<TModel> BuildSort() {
            if (this.Sorts.Count == 0)
                return SortBuilder.Descending("_id");

            return SortBuilder.Combine(this.Sorts);
        }

        protected override int? BuildSkip() {
            return this.Skip;
        }

        protected override int? BuildLimit() {
            return this.Limit;
        }

        public virtual TBuilder Page(int index, int size = 15) {
            this.Skip = (index - 1) * size;
            this.Limit = size;
            return this as TBuilder;
        }

        public virtual TBuilder SortAsc(string value) {
            this.Sorts.Add(SortBuilder.Ascending(value));
            return this as TBuilder;
        }

        public virtual TBuilder SortDesc(string value) {
            this.Sorts.Add(SortBuilder.Descending(value));
            return this as TBuilder;
        }
    }
}
