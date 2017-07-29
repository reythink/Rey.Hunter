using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
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
    }
}
