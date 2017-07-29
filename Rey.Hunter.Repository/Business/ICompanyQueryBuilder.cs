using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Enums;

namespace Rey.Hunter.Repository.Business {
    public interface ICompanyQueryBuilder : IQueryBuilder<Company, ICompanyQueryBuilder> {
        ICompanyQueryBuilder FilterName(params string[] values);
        ICompanyQueryBuilder FilterIndustry(params string[] values);
        ICompanyQueryBuilder FilterType(params CompanyType[] values);
        ICompanyQueryBuilder FilterStatus(params CompanyStatus[] values);
    }
}
