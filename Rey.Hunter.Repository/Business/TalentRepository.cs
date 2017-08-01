using System;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;

namespace Rey.Hunter.Repository.Business {
    public class TalentRepository : AccountRepositoryBase<Talent>, ITalentRepository {
        public TalentRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public ITalentQueryBuilder Query() {
            return new TalentQueryBuilder(this);
        }

        public override void UpdateRef(Talent model) {
            throw new NotImplementedException();
        }
    }
}
