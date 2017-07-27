using Rey.Hunter.Models2.Basic;

namespace Rey.Hunter.Repository.Repositories {
    public interface IIndustryRepository : IAccountModelRepository<Industry> {
        void Initialize();
    }
}
