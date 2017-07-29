using Rey.Hunter.Models2.Business;

namespace Rey.Hunter.Repository.Business {
    public interface IProjectQueryBuilder : IQueryBuilder<Project, IProjectQueryBuilder> {
        IProjectQueryBuilder FilterPosition(params string[] values);
        IProjectQueryBuilder FilterClientName(params string[] values);
        IProjectQueryBuilder FilterFunction(params string[] values);
        IProjectQueryBuilder FilterManager(params string[] values);
        IProjectQueryBuilder FilterConsultant(params string[] values);
    }
}
