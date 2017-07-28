using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Data {
    public class FunctionRepository : AccountModelRepositoryBase<Function>, IFunctionRepository {
        public FunctionRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }
    }
}
