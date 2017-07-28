using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Data {
    public class CategoryRepository : AccountModelRepositoryBase<Category>, ICategoryRepository {
        public CategoryRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }
    }
}
