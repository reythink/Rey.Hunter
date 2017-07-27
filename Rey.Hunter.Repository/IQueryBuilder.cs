using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rey.Hunter.Repository {
    public interface IQueryBuilder<TModel> {
        //IQueryBuilder<TModel> Aggregate(Func<IAggregateFluent<BsonDocument>, IAggregateFluent<BsonDocument>> build);
        //IQueryBuilder<TModel> Filter(Func<FilterDefinitionBuilder<BsonDocument>, FilterDefinition<BsonDocument>> build);
        IEnumerable<TModel> ToEnumerable();
    }

    public interface ICompanyQueryBuilder : IQueryBuilder<Company> {
        ICompanyQueryBuilder IndustryName(string value);
    }

    public class CompanyQueryBuilder : QueryBuilder<Company>, ICompanyQueryBuilder {
        private List<string> IndustryNameList { get; } = new List<string>();

        public CompanyQueryBuilder(IRepository<Company> repository)
            : base(repository) {
        }

        public ICompanyQueryBuilder IndustryName(string value) {
            this.IndustryNameList.Add(value);
            return this;
        }

        public FilterDefinition<BsonDocument> BuildFilter() {
            var filters = new List<FilterDefinition<BsonDocument>>();
            filters.Add(Builders<BsonDocument>.Filter.Or(this.IndustryNameList.Select(x => Builders<BsonDocument>.Filter.Regex("industry.name", new BsonRegularExpression(x, "i")))));
            return Builders<BsonDocument>.Filter.And(filters);
        }

        public override IEnumerable<Company> ToEnumerable() {
            var agg = this.Repository.DocCollectin
                .Aggregate()
                .Lookup("industry", "industry.@id", "_id", "industry")
                .Match(this.BuildFilter());

            return agg.Project(Builders<BsonDocument>.Projection.As<Company>()).ToEnumerable();
        }
    }
}
