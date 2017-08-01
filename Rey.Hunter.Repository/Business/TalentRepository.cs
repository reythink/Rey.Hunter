using System;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;

namespace Rey.Hunter.Repository.Business {
    public class TalentRepository : AccountRepositoryBase<Talent>, ITalentRepository {
        public TalentRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }

        public ITalentQueryBuilder Query() {
            return new TalentQueryBuilder(this);
        }

        public override void UpdateRef(Talent model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            using (var tool = new UpdateTool<Talent>(this, model)) {
                tool.Update(x => x.Account);
                tool.Update(x => x.Industry);
                tool.Update(x => x.Function);
                tool.Update(x => x.Location.Current);
                tool.Update(x => x.Location.Mobility);
            }
        }
    }
}
