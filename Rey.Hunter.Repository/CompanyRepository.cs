using Rey.Hunter.Models2;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rey.Hunter.Repository {
    public class CompanyRepository : AccountModelRepositoryBase<Company>, ICompanyRepository {
        public CompanyRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }

        public override string GetCollectionName() {
            return "company";
        }
    }
}
