using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rey.Hunter.Repository {
    public class QueryResult {
        public long Count { get; }

        public QueryResult(long count) {
            this.Count = count;
        }
    }

    public interface IQueryBuilder<TModel> {
        IEnumerable<TModel> Build(Action<QueryResult> result = null);
    }

    public interface ICompanyQueryBuilder : IQueryBuilder<Company> {
        ICompanyQueryBuilder Name(params string[] values);
        ICompanyQueryBuilder IndustryName(params string[] values);

        ICompanyQueryBuilder SortAsc(string value);
        ICompanyQueryBuilder SortDesc(string value);

        ICompanyQueryBuilder Page(int index, int size = 15);
    }

    public abstract class QueryBuilder<TModel> : IQueryBuilder<TModel> {
        protected IRepository<TModel> Repository { get; }
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

    public class CompanyQueryBuilder : QueryBuilder<Company>, ICompanyQueryBuilder {
        private List<SortDefinition<Company>> Sorts { get; } = new List<SortDefinition<Company>>();
        private List<FilterDefinition<Company>> NameFilters { get; } = new List<FilterDefinition<Company>>();
        private List<FilterDefinition<Company>> IndustryNameFilters { get; } = new List<FilterDefinition<Company>>();

        private int? Skip { get; set; }
        private int? Limit { get; set; }

        public CompanyQueryBuilder(IRepository<Company> repository)
            : base(repository) {
        }

        public ICompanyQueryBuilder Name(params string[] values) {
            this.NameFilters.AddRange(values.Select(x => Builders<Company>.Filter.Regex("name", new BsonRegularExpression(x, "i"))));
            return this;
        }

        public ICompanyQueryBuilder IndustryName(params string[] values) {
            this.IndustryNameFilters.AddRange(values.Select(x => Builders<Company>.Filter.Regex("industry.name", new BsonRegularExpression(x, "i"))));
            return this;
        }

        public ICompanyQueryBuilder SortAsc(string value) {
            this.Sorts.Add(Builders<Company>.Sort.Ascending(value));
            return this;
        }

        public ICompanyQueryBuilder SortDesc(string value) {
            this.Sorts.Add(Builders<Company>.Sort.Descending(value));
            return this;
        }

        public ICompanyQueryBuilder Page(int index, int size = 15) {
            this.Skip = (index - 1) * size;
            this.Limit = size;
            return this;
        }

        protected override FilterDefinition<Company> BuildFilter() {
            var filters = new List<FilterDefinition<Company>>();
            BuildFilter(filters, this.NameFilters);
            BuildFilter(filters, this.IndustryNameFilters);

            if (filters.Count == 0)
                return Builders<Company>.Filter.Empty;

            return Builders<Company>.Filter.And(filters);
        }

        private void BuildFilter(ICollection<FilterDefinition<Company>> filters, IEnumerable<FilterDefinition<Company>> subs) {
            if (subs.Count() > 0) {
                filters.Add(Builders<Company>.Filter.Or(subs));
            }
        }

        protected override SortDefinition<Company> BuildSort() {
            if (this.Sorts.Count == 0)
                return Builders<Company>.Sort.Descending("_id");

            return Builders<Company>.Sort.Combine(this.Sorts);
        }

        protected override int? BuildSkip() {
            return this.Skip;
        }

        protected override int? BuildLimit() {
            return this.Limit;
        }
    }
}
