using Rey.Hunter.Models2.Business;

namespace Rey.Hunter.Repository.Business {
    public interface ITalentRepository : IAccountModelRepository<Talent> {
        ITalentQueryBuilder Query();
    }
}
