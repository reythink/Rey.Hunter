using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;
using System.Linq;
using System;

namespace Rey.Hunter.Repository.Business {
    public class TalentQueryBuilder : QueryBuilder<Talent, ITalentQueryBuilder>, ITalentQueryBuilder {
        public TalentQueryBuilder(IRepository<Talent> repository)
            : base(repository) {
        }

        public ITalentQueryBuilder FilterCompanyName(params string[] values) {
            return this.AddFilters("FilterCompanyName", values.Select(value => {
                var filterCurrent = Builders<TalentExperience>.Filter.Eq(x => x.Current, true);
                var filterCompanyName = Builders<TalentExperience>.Filter.Regex(x => x.Company.Name, new BsonRegularExpression(value, "i"));
                var filterElem = Builders<TalentExperience>.Filter.And(filterCurrent, filterCompanyName);
                return FilterBuilder.ElemMatch(x => x.Experience, filterElem);
            }));
        }

        public ITalentQueryBuilder FilterPreviousCompanyName(params string[] values) {
            return this.AddFilters("FilterPreviousCompanyName", values.Select(value => {
                var filterNotCurrent = Builders<TalentExperience>.Filter.Eq(x => x.Current, false);
                var filterCompanyName = Builders<TalentExperience>.Filter.Regex(x => x.Company.Name, new BsonRegularExpression(value, "i"));
                var filterElem = Builders<TalentExperience>.Filter.And(filterNotCurrent, filterCompanyName);
                return FilterBuilder.ElemMatch(x => x.Experience, filterElem);
            }));
        }

        public ITalentQueryBuilder FilterTitle(params string[] values) {
            return this.AddFilters("FilterTitle", values.Select(value => {
                var filterCurrent = Builders<TalentExperience>.Filter.Eq(x => x.Current, true);
                var filterTitle = Builders<TalentExperience>.Filter.Regex(x => x.Title, new BsonRegularExpression(value, "i"));
                var filterElem = Builders<TalentExperience>.Filter.And(filterCurrent, filterTitle);
                return FilterBuilder.ElemMatch(x => x.Experience, filterElem);
            }));
        }

        public ITalentQueryBuilder FilterResponsibility(params string[] values) {
            return this.AddFilters("FilterResponsibility", values.Select(value => {
                var filterCurrent = Builders<TalentExperience>.Filter.Eq(x => x.Current, true);
                var filterResponsibility = Builders<TalentExperience>.Filter.Regex(x => x.Responsibility, new BsonRegularExpression(value, "i"));
                var filterElem = Builders<TalentExperience>.Filter.And(filterCurrent, filterResponsibility);
                return FilterBuilder.ElemMatch(x => x.Experience, filterElem);
            }));
        }

        public ITalentQueryBuilder FilterGrade(params string[] values) {
            return this.AddFilters("FilterGrade", values.Select(value => {
                var filterCurrent = Builders<TalentExperience>.Filter.Eq(x => x.Current, true);
                var filterGrade = Builders<TalentExperience>.Filter.Regex(x => x.Grade, new BsonRegularExpression(value, "i"));
                var filterElem = Builders<TalentExperience>.Filter.And(filterCurrent, filterGrade);
                return FilterBuilder.ElemMatch(x => x.Experience, filterElem);
            }));
        }

        public ITalentQueryBuilder FilterIndustry(params string[] values) {
            return this.AddFilter("FilterIndustry",
                    FilterBuilder.ElemMatch(x => x.Industry, Builders<IndustryRef>.Filter.In(x => x.Id, values))
                );
        }

        public ITalentQueryBuilder FilterCrossIndustry(params string[] values) {
            return this.AddFilter("FilterCrossIndustry",
                    FilterBuilder.ElemMatch(x => x.Profile.CrossIndustry, Builders<IndustryRef>.Filter.In(x => x.Id, values))
                );
        }

        public ITalentQueryBuilder FilterFunction(params string[] values) {
            return this.AddFilter("FilterFunction",
                    FilterBuilder.ElemMatch(x => x.Function, Builders<FunctionRef>.Filter.In(x => x.Id, values))
                );
        }

        public ITalentQueryBuilder FilterCrossFunction(params string[] values) {
            return this.AddFilter("FilterCrossFunction",
                    FilterBuilder.ElemMatch(x => x.Profile.CrossFunction, Builders<FunctionRef>.Filter.In(x => x.Id, values))
                );
        }
    }
}
