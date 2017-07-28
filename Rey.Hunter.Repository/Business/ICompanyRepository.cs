using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository.Business {
    public interface ICompanyRepository : IAccountModelRepository<Company> {
        ICompanyQueryBuilder Query();
    }
}
