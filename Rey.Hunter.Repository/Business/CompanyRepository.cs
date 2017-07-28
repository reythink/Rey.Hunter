using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;

namespace Rey.Hunter.Repository.Business {
    public class CompanyRepository : AccountModelRepositoryBase<Company>, ICompanyRepository {
        public CompanyRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }

        public ICompanyQueryBuilder Query() {
            return new CompanyQueryBuilder(this);
        }
    }
}
