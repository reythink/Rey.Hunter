using System;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;

namespace Rey.Hunter.Repository.Business {
    public class ProjectRepository : AccountRepositoryBase<Project>, IProjectRepository {
        public ProjectRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public IProjectQueryBuilder Query() {
            return new ProjectQueryBuilder(this);
        }

        public override void UpdateRef(Project model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            using (var tool = new UpdateTool<Project>(this, model)) {
                tool.Update(x => x.Account);
                tool.Update(x => x.Client);
                tool.Update(x => x.Manager);
                tool.Update(x => x.Consultant);
                tool.Update(x => x.Function);
                tool.Update(x => x.Location);
                tool.Update(x => x.Candidate, x => x.Talent);
            }
        }
    }
}
