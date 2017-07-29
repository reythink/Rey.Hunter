using Rey.Hunter.Models2.Business;

namespace Rey.Hunter.Repository.Business {
    public interface ICompanyQueryBuilder : IQueryBuilder<Company, ICompanyQueryBuilder> {
        ICompanyQueryBuilder FilterName(params string[] values);
        ICompanyQueryBuilder FilterIndustryName(params string[] values);
        ICompanyQueryBuilder FilterType(params int[] values);
        ICompanyQueryBuilder FilterStatus(params int[] values);
    }
}
