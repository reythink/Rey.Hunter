namespace Rey.Hunter.Models2 {
    public interface IAccountModel : IModel {
        ModelRef<Account> Account { get; set; }
    }
}
