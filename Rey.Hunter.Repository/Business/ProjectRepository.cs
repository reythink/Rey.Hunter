﻿using System;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;

namespace Rey.Hunter.Repository.Business {
    public class ProjectRepository : AccountModelRepositoryBase<Project>, IProjectRepository {
        public ProjectRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public IProjectQueryBuilder Query() {
            return new ProjectQueryBuilder(this);
        }

        public override void UpdateRef(Project model) {
            throw new NotImplementedException();
        }
    }
}
