﻿using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository.Auth {
    public class RoleRepository : AccountModelRepositoryBase<Role>, IRoleRepository {
        public RoleRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }
    }
}
