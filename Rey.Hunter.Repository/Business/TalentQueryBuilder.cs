using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;
using System.Linq;
using System;
using Rey.Hunter.Models2.Enums;

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

        public ITalentQueryBuilder FilterCrossCategory(params string[] values) {
            return this.AddFilter("FilterCrossCategory",
                     FilterBuilder.ElemMatch(x => x.Profile.CrossCategory, Builders<CategoryRef>.Filter.In(x => x.Id, values))
                 );
        }

        public ITalentQueryBuilder FilterCrossChannel(params string[] values) {
            return this.AddFilter("FilterCrossChannel",
                     FilterBuilder.ElemMatch(x => x.Profile.CrossChannel, Builders<ChannelRef>.Filter.In(x => x.Id, values))
                 );
        }

        public ITalentQueryBuilder FilterBrand(params string[] values) {
            return this.AddFilters("FilterBrand", values.Select(value => {
                return FilterBuilder.Regex(x => x.Profile.Brand, new BsonRegularExpression(value, "i"));
            }));
        }

        public ITalentQueryBuilder FilterKeyAccount(params string[] values) {
            return this.AddFilters("FilterKeyAccount", values.Select(value => {
                return FilterBuilder.Regex(x => x.Profile.KeyAccount, new BsonRegularExpression(value, "i"));
            }));
        }

        public ITalentQueryBuilder FilterCurrentLocation(params string[] values) {
            return this.AddFilter("FilterCurrentLocation",
                    FilterBuilder.In(x => x.Location.Current.Id, values)
                );
        }

        public ITalentQueryBuilder FilterMobilityLocation(params string[] values) {
            return this.AddFilter("FilterMobilityLocation",
                    FilterBuilder.ElemMatch(x => x.Location.Mobility, Builders<LocationRef>.Filter.In(x => x.Id, values))
                );
        }

        public ITalentQueryBuilder FilterGender(params Gender[] values) {
            return this.AddFilter("FilterGender",
                    FilterBuilder.In(x => x.Gender, values.Select(x => (Gender?)x))
                );
        }

        public ITalentQueryBuilder FilterEducation(params Education[] values) {
            return this.AddFilter("FilterEducation",
                    FilterBuilder.In(x => x.Education, values.Select(x => (Education?)x))
                );
        }

        public ITalentQueryBuilder FilterLanguage(params Language[] values) {
            return this.AddFilter("FilterLanguage",
                    FilterBuilder.In(x => x.Language, values.Select(x => (Language?)x))
                );
        }

        public ITalentQueryBuilder FilterNationality(params Nationality[] values) {
            return this.AddFilter("FilterNationality",
                    FilterBuilder.In(x => x.Nationality, values.Select(x => (Nationality?)x))
                );
        }

        public ITalentQueryBuilder FilterIntension(params Intension[] values) {
            return this.AddFilter("FilterIntension",
                    FilterBuilder.In(x => x.Intension, values.Select(x => (Intension?)x))
                );
        }
    }
}
