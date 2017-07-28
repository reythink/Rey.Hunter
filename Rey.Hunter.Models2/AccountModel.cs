namespace Rey.Hunter.Models2 {
    public abstract class AccountModel : Model, IAccountModel {
        public AccountRef Account { get; set; }
    }
}
