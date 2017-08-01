namespace Rey.Hunter.Models2.Data {
    public class ChannelRef : NodeModelRef<Channel> {
        public ChannelRef(Channel model)
            : base(model) {
        }

        public static implicit operator ChannelRef(Channel model) {
            if (model == null)
                return null;

            return new ChannelRef(model);
        }
    }
}
