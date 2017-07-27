using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Repositories {
    public interface IIndustryRepository : IAccountModelRepository<Industry> {
        void Initialize();
    }
}
