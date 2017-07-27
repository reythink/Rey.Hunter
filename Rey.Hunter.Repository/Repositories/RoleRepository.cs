﻿using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository.Repositories {
    public class RoleRepository : AccountModelRepositoryBase<Role>, IRoleRepository {
        public RoleRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }
    }
}