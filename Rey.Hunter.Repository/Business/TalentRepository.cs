using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository.Business {
    public class TalentRepository : AccountModelRepositoryBase<Talent>, ITalentRepository {
        public TalentRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }
    }
}
