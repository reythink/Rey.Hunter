using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository.Auth {
    public class UserRepository : AccountModelRepositoryBase<User>, IUserRepository {
        public UserRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }
    }
}
