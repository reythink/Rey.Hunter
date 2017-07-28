using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;

namespace Rey.Hunter.Repository.Business {
    public class ProjectRepository : AccountModelRepositoryBase<Project>, IProjectRepository {
        public ProjectRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }
    }
}
