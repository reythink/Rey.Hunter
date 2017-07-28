using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;

namespace Rey.Hunter.Repository.Business {
    public class CompanyRepository : AccountModelRepositoryBase<Company>, ICompanyRepository {
        public CompanyRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public ICompanyQueryBuilder Query() {
            return new CompanyQueryBuilder(this);
        }
    }
}
