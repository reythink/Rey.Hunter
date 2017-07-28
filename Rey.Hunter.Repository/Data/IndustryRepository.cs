using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Data {
    public class IndustryRepository : AccountModelRepositoryBase<Industry>, IIndustryRepository {
        public IndustryRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }
    }
}
