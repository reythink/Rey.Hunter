using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Enums;
using Rey.Hunter.Repository.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rey.Hunter.Repository.Business {
    public interface ITalentQueryBuilder : IQueryBuilder<Talent, ITalentQueryBuilder> {
        ITalentQueryBuilder FilterCompanyName(params string[] values);
        ITalentQueryBuilder FilterPreviousCompanyName(params string[] values);
        ITalentQueryBuilder FilterTitle(params string[] values);
        ITalentQueryBuilder FilterResponsibility(params string[] values);
        ITalentQueryBuilder FilterGrade(params string[] values);
        ITalentQueryBuilder FilterIndustry(params string[] values);
        ITalentQueryBuilder FilterCrossIndustry(params string[] values);
        ITalentQueryBuilder FilterFunction(params string[] values);
        ITalentQueryBuilder FilterCrossFunction(params string[] values);
        ITalentQueryBuilder FilterCrossCategory(params string[] values);
        ITalentQueryBuilder FilterCrossChannel(params string[] values);
        ITalentQueryBuilder FilterBrand(params string[] values);
        ITalentQueryBuilder FilterKeyAccount(params string[] values);
        ITalentQueryBuilder FilterCurrentLocation(params string[] values);
        ITalentQueryBuilder FilterMobilityLocation(params string[] values);
        ITalentQueryBuilder FilterGender(params Gender[] values);
        ITalentQueryBuilder FilterEducation(params Education[] values);
        ITalentQueryBuilder FilterLanguage(params Language[] values);
        ITalentQueryBuilder FilterNationality(params Nationality[] values);
        ITalentQueryBuilder FilterIntension(params Intension[] values);
    }
}
