using System;
using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository.Auth {
    public class UserRepository : AccountRepositoryBase<User>, IUserRepository {
        public UserRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public override void UpdateRef(User model) {
            throw new NotImplementedException();
        }
    }
}
