using Rey.Hunter.Models2;

namespace Rey.Hunter.Repository {
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository {
        public AccountRepository(IRepositoryManager manager)
            : base(manager) {
        }

        public override string GetCollectionName() {
            return "account";
        }
    }
}
