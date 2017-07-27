using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository.Repositories {
    public class UserRepository : AccountModelRepositoryBase<User>, IUserRepository {
        public UserRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }
    }
}
