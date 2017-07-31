using System;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Data {
    public class LocationRepository : AccountNodeModelRepositoryBase<Location>, ILocationRepository {
        public LocationRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public override void UpdateRef(Location model) {
            throw new NotImplementedException();
        }
    }
}
