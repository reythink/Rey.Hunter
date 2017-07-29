using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Data {
    public class ChannelRepository : AccountNodeModelRepositoryBase<Channel>, IChannelRepository {
        public ChannelRepository(IRepositoryManager manager, Account account)
            : base(manager, account) {
        }
    }
}
