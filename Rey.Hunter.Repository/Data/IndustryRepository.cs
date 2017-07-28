using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Data {
    public class IndustryRepository : AccountModelRepositoryBase<Industry>, IIndustryRepository {
        public IndustryRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }
    }
}
