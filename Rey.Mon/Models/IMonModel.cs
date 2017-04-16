namespace Rey.Mon.Models {
    public interface IMonModel<TKey> {
        TKey Id { get; set; }
        bool IsIdEmpty();
    }
}
