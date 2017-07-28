using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository.Auth {
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository {
        public AccountRepository(IRepositoryManager manager)
            : base(manager) {
        }
    }
}
