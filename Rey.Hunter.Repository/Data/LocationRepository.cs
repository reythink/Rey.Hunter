using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Data {
    public class LocationRepository : AccountModelRepositoryBase<Location>, ILocationRepository {
        public LocationRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }
    }
}
