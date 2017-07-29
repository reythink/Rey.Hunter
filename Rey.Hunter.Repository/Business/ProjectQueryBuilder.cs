using Rey.Hunter.Models2.Business;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models2.Data;
using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository.Business {
    public class ProjectQueryBuilder : QueryBuilder<Project, IProjectQueryBuilder>, IProjectQueryBuilder {
        public ProjectQueryBuilder(IRepository<Project> repository)
            : base(repository) {
        }

        public IProjectQueryBuilder FilterPosition(params string[] values) {
            return this.AddFilters("FilterPosition", values.Select(value => {
                return FilterBuilder.Regex(x => x.Position, new BsonRegularExpression(value, "i"));
            }));
        }

        public IProjectQueryBuilder FilterClientName(params string[] values) {
            return this.AddFilters("FilterClientName", values.Select(value => {
                return FilterBuilder.Regex(x => x.Client.Name, new BsonRegularExpression(value, "i"));
            }));
        }

        public IProjectQueryBuilder FilterFunction(params string[] values) {
            return this.AddFilter("FilterFunction",
                    FilterBuilder.ElemMatch(x => x.Function, Builders<FunctionRef>.Filter.In(x => x.Id, values))
                );
        }

        public IProjectQueryBuilder FilterManager(params string[] values) {
            return this.AddFilter("FilterManager",
                    FilterBuilder.In(x => x.Manager.Id, values)
                );
        }

        public IProjectQueryBuilder FilterConsultant(params string[] values) {
            return this.AddFilter("FilterConsultant",
                    FilterBuilder.ElemMatch(x => x.Consultant, Builders<UserRef>.Filter.In(x => x.Id, values))
                );
        }
    }
}
