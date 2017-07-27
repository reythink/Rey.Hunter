namespace Rey.Hunter.Models2 {
    public abstract class AccountModel : Model, IAccountModel {
        public ModelRef<Account> Account { get; set; }
    }
}
