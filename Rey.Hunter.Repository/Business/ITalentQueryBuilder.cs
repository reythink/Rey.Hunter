using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models2.Business;
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
    }

    public class TalentQueryBuilder : QueryBuilder<Talent, ITalentQueryBuilder>, ITalentQueryBuilder {
        public TalentQueryBuilder(IRepository<Talent> repository)
            : base(repository) {
        }

        public ITalentQueryBuilder FilterCompanyName(params string[] values) {
            return this.AddFilters("CompanyName", values.Select(value => {
                var filterCurrent = Builders<TalentExperience>.Filter.Eq(x => x.Current, true);
                var filterCompanyName = Builders<TalentExperience>.Filter.Regex(x => x.Company.Name, new BsonRegularExpression(value, "i"));
                var filterElem = Builders<TalentExperience>.Filter.And(filterCurrent, filterCompanyName);
                return FilterBuilder.ElemMatch(x => x.Experience, filterElem);
            }));
        }

        public ITalentQueryBuilder FilterPreviousCompanyName(params string[] values) {
            return this.AddFilters("PreviousCompanyName", values.Select(value => {
                var filterNotCurrent = Builders<TalentExperience>.Filter.Eq(x => x.Current, false);
                var filterCompanyName = Builders<TalentExperience>.Filter.Regex(x => x.Company.Name, new BsonRegularExpression(value, "i"));
                var filterElem = Builders<TalentExperience>.Filter.And(filterNotCurrent, filterCompanyName);
                return FilterBuilder.ElemMatch(x => x.Experience, filterElem);
            }));
        }

        public ITalentQueryBuilder FilterTitle(params string[] values) {
            return this.AddFilters("CompanyName", values.Select(value => {
                var filterCurrent = Builders<TalentExperience>.Filter.Eq(x => x.Current, true);
                var filterTitle = Builders<TalentExperience>.Filter.Regex(x => x.Title, new BsonRegularExpression(value, "i"));
                var filterElem = Builders<TalentExperience>.Filter.And(filterCurrent, filterTitle);
                return FilterBuilder.ElemMatch(x => x.Experience, filterElem);
            }));
        }

        public ITalentQueryBuilder FilterResponsibility(params string[] values) {
            return this.AddFilters("Responsibility", values.Select(value => {
                var filterCurrent = Builders<TalentExperience>.Filter.Eq(x => x.Current, true);
                var filterResponsibility = Builders<TalentExperience>.Filter.Regex(x => x.Responsibility, new BsonRegularExpression(value, "i"));
                var filterElem = Builders<TalentExperience>.Filter.And(filterCurrent, filterResponsibility);
                return FilterBuilder.ElemMatch(x => x.Experience, filterElem);
            }));
        }

        public ITalentQueryBuilder FilterGrade(params string[] values) {
            return this.AddFilters("Grade", values.Select(value => {
                var filterCurrent = Builders<TalentExperience>.Filter.Eq(x => x.Current, true);
                var filterGrade = Builders<TalentExperience>.Filter.Regex(x => x.Grade, new BsonRegularExpression(value, "i"));
                var filterElem = Builders<TalentExperience>.Filter.And(filterCurrent, filterGrade);
                return FilterBuilder.ElemMatch(x => x.Experience, filterElem);
            }));
        }
    }
}
