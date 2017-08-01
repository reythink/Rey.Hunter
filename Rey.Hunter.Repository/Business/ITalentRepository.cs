using Rey.Hunter.Models2.Business;

namespace Rey.Hunter.Repository.Business {
    public interface ITalentRepository : IAccountRepository<Talent> {
        ITalentQueryBuilder Query();
    }
}
