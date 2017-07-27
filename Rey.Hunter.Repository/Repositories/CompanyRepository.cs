using System;
using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository.Repositories {
    public class CompanyRepository : AccountModelRepositoryBase<Company>, ICompanyRepository {
        public CompanyRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }

        public ICompanyQueryBuilder Query() {
            return new CompanyQueryBuilder(this);
        }
    }
}
