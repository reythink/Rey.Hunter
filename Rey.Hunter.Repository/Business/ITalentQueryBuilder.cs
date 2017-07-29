using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;
using Rey.Hunter.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rey.Hunter.Repository.Business {
    public interface ITalentQueryBuilder : IQueryBuilder<Talent, ITalentQueryBuilder> {
        ITalentQueryBuilder FilterCompanyName(params string[] values);
        ITalentQueryBuilder FilterPreviousCompanyName(params string[] values);
        ITalentQueryBuilder FilterTitle(params string[] values);
        ITalentQueryBuilder FilterResponsibility(params string[] values);
        ITalentQueryBuilder FilterGrade(params string[] values);
        ITalentQueryBuilder FilterIndustry(params string[] values);
        ITalentQueryBuilder FilterIndustryWithChildren(params string[] values);
    }

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

        public ITalentQueryBuilder FilterIndustryWithChildren(params string[] values) {
            throw new NotImplementedException();
            //return this.AddFilters("FilterIndustryWithChildren", values.Select(value => {
            //    var filterName = Builders<IndustryRef>.Filter.Regex(x => x.Name, new BsonRegularExpression(value, "i"));
            //    var filterPathName = Builders<NodeModelRef>.Filter.Regex(x => x.Name, new BsonRegularExpression(value, "i"));
            //    var filter = Builders<IndustryRef>.Filter.Or(filterName, Builders<IndustryRef>.Filter.ElemMatch(x => x.Path, filterPathName));
            //    return FilterBuilder.ElemMatch(x => x.Industry, filter);
            //}));
        }
    }
}
