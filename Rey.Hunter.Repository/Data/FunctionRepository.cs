using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Data {
    public class FunctionRepository : AccountModelRepositoryBase<Function>, IFunctionRepository {
        public FunctionRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }
    }
}
