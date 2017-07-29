using MongoDB.Bson;
using Rey.Hunter.Models2.Business;
using System.Linq;

namespace Rey.Hunter.Repository.Business {
    public class CompanyQueryBuilder : QueryBuilder<Company, ICompanyQueryBuilder>, ICompanyQueryBuilder {
        public CompanyQueryBuilder(IRepository<Company> repository)
            : base(repository) {
        }

        public ICompanyQueryBuilder FilterName(params string[] values) {
            return AddFilters("Name", values.Select(x => FilterBuilder.Regex("name", new BsonRegularExpression(x, "i"))));
        }

        public ICompanyQueryBuilder FilterIndustryName(params string[] values) {
            return this.AddFilters("IndustryName", values.Select(x => FilterBuilder.Regex("industry.name", new BsonRegularExpression(x, "i"))));
        }

        public ICompanyQueryBuilder FilterType(params int[] values) {
            return this.AddFilters("Type", values.Select(x => FilterBuilder.Eq("type", x)));
        }

        public ICompanyQueryBuilder FilterStatus(params int[] values) {
            return this.AddFilters("Status", values.Select(x => FilterBuilder.Eq("status", x)));
        }
    }
}
