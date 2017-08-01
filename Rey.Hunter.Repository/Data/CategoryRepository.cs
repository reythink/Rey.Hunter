using System;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Data {
    public class CategoryRepository : AccountNodeRepositoryBase<Category, CategoryRef>, ICategoryRepository {
        public CategoryRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public override void UpdateRef(Category model) {
            throw new NotImplementedException();
        }
    }
}
