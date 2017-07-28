using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Rey.Hunter.Models2;
using System.Collections.Generic;
using System.Linq;

namespace Rey.Hunter.Repository {
    public interface IQueryBuilder<TModel> {
        IEnumerable<TModel> Build();
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

        public abstract IEnumerable<TModel> Build();
    }

    public class CompanyQueryBuilder : QueryBuilder<Company>, ICompanyQueryBuilder {
        private List<SortDefinition<BsonDocument>> Sorts { get; } = new List<SortDefinition<BsonDocument>>();
        private List<FilterDefinition<BsonDocument>> NameFilters { get; } = new List<FilterDefinition<BsonDocument>>();
        private List<FilterDefinition<BsonDocument>> IndustryNameFilters { get; } = new List<FilterDefinition<BsonDocument>>();

        private int PageIndex { get; set; } = 1;
        private int PageSize { get; set; } = 15;

        public CompanyQueryBuilder(IRepository<Company> repository)
            : base(repository) {
        }

        public ICompanyQueryBuilder Name(params string[] values) {
            this.NameFilters.AddRange(values.Select(x => Builders<BsonDocument>.Filter.Regex("name", new BsonRegularExpression(x, "i"))));
            return this;
        }

        public ICompanyQueryBuilder IndustryName(params string[] values) {
            this.IndustryNameFilters.AddRange(values.Select(x => Builders<BsonDocument>.Filter.Regex("industry.name", new BsonRegularExpression(x, "i"))));
            return this;
        }

        public ICompanyQueryBuilder SortAsc(string value) {
            this.Sorts.Add(Builders<BsonDocument>.Sort.Ascending(value));
            return this;
        }

        public ICompanyQueryBuilder SortDesc(string value) {
            this.Sorts.Add(Builders<BsonDocument>.Sort.Descending(value));
            return this;
        }

        public ICompanyQueryBuilder Page(int index, int size = 15) {
            this.PageIndex = index;
            this.PageSize = size;
            return this;
        }

        private FilterDefinition<BsonDocument> BuildFilter() {
            var filters = new List<FilterDefinition<BsonDocument>>();
            BuildFilter(filters, this.NameFilters);
            BuildFilter(filters, this.IndustryNameFilters);

            if (filters.Count == 0)
                return Builders<BsonDocument>.Filter.Empty;

            return Builders<BsonDocument>.Filter.And(filters);
        }

        private void BuildFilter(ICollection<FilterDefinition<BsonDocument>> filters, IEnumerable<FilterDefinition<BsonDocument>> subs) {
            if (subs.Count() > 0) {
                filters.Add(Builders<BsonDocument>.Filter.Or(subs));
            }
        }

        private SortDefinition<BsonDocument> BuildSort() {
            if (this.Sorts.Count == 0)
                return Builders<BsonDocument>.Sort.Descending("_id");

            return Builders<BsonDocument>.Sort.Combine(this.Sorts);
        }

        public override IEnumerable<Company> Build() {
            var agg = this.Repository.BsonCollection
                .Aggregate()
                .Lookup("industry", "industry._id", "_id", "industry")
                //.Sort(this.BuildSort())
                .Match(this.BuildFilter());

            var count = agg.Count().SingleOrDefault()?.Count ?? 0;

            var list = agg
                .Sort(this.BuildSort())
                .Skip((this.PageIndex - 1) * this.PageSize)
                .Limit(this.PageSize)
                .ToEnumerable()
                .Select(x => BsonSerializer.Deserialize<Company>(x));

            return list;
        }
    }
}
