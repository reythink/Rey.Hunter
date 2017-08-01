using System;

namespace Rey.Hunter.Models2 {
    public class AccountRef : ModelRef<Account> {
        public AccountRef(Account model)
            : base(model) {
        }

        public static implicit operator AccountRef(Account model) {
            if (model == null)
                return null;

            return new AccountRef(model);
        }
    }
}

