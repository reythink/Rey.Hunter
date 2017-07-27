using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository.Repositories {
    public interface ICompanyRepository : IAccountModelRepository<Company> {
        ICompanyQueryBuilder Query();
    }
}
