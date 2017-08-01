using System;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Data {
    public class FunctionRepository : AccountNodeRepositoryBase<Function, FunctionRef>, IFunctionRepository {
        public FunctionRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public override void UpdateRef(Function model) {
            throw new NotImplementedException();
        }
    }
}
