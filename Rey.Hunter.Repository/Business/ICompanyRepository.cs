using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;

namespace Rey.Hunter.Repository.Business {
    public interface ICompanyRepository : IAccountModelRepository<Company> {
        ICompanyQueryBuilder Query();
    }
}
