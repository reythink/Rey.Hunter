using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;
using Rey.Hunter.Models2.Enums;
using System.Linq;

namespace Rey.Hunter.Repository.Business {
    public class CompanyQueryBuilder : QueryBuilder<Company, ICompanyQueryBuilder>, ICompanyQueryBuilder {
        public CompanyQueryBuilder(IRepository<Company> repository)
            : base(repository) {
        }

        public ICompanyQueryBuilder FilterName(params string[] values) {
            return this.AddFilters("FilterName", values.Select(value => {
                return FilterBuilder.Regex(x => x.Name, new BsonRegularExpression(value, "i"));
            }));
        }

        public ICompanyQueryBuilder FilterIndustry(params string[] values) {
            return this.AddFilter("FilterIndustry",
                    FilterBuilder.ElemMatch(x => x.Industry, Builders<IndustryRef>.Filter.In(x => x.Id, values))
                );
        }

        public ICompanyQueryBuilder FilterType(params CompanyType[] values) {
            return this.AddFilter("FilterType",
                    FilterBuilder.In(x => x.Type, values.Select(x => (CompanyType?)x))
                );
        }

        public ICompanyQueryBuilder FilterStatus(params CompanyStatus[] values) {
            return this.AddFilter("FilterStatus",
                    FilterBuilder.In(x => x.Status, values.Select(x => (CompanyStatus?)x))
                );
        }
    }
}
