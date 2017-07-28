using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Data {
    public class LocationRepository : AccountModelRepositoryBase<Location>, ILocationRepository {
        public LocationRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }
    }
}
