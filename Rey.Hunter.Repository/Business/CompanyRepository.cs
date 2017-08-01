using System;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Business {
    public class CompanyRepository : AccountRepositoryBase<Company>, ICompanyRepository {
        public CompanyRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public ICompanyQueryBuilder Query() {
            return new CompanyQueryBuilder(this);
        }

        public override void UpdateRef(Company model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            using (var tool = new UpdateTool<Company>(this, model)) {
                tool.Update(x => x.Account);
                tool.Update(x => x.Industry);
            }
        }
    }
}
