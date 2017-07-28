using Rey.Hunter.Models2.Data;

namespace Rey.Hunter.Repository.Data {
    public class ChannelRepository : AccountModelRepositoryBase<Channel>, IChannelRepository {
        public ChannelRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }
    }
}
