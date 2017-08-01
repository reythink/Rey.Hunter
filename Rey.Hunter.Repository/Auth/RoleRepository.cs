using System;
using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository.Auth {
    public class RoleRepository : AccountRepositoryBase<Role>, IRoleRepository {
        public RoleRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public override void UpdateRef(Role model) {
            throw new NotImplementedException();
        }
    }
}
