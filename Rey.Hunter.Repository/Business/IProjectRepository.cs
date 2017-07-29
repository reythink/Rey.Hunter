using System;
using Rey.Hunter.Models2.Business;

namespace Rey.Hunter.Repository.Business {
    public interface IProjectRepository : IAccountModelRepository<Project> {
        IProjectQueryBuilder Query();
    }
}
