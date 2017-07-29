using System;

namespace Rey.Hunter.Models2 {
    public class AccountRef : ModelRef {
        public AccountRef(Account model) {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            this.Id = model.Id;
        }

        public static implicit operator AccountRef(Account model) {
            if (model == null)
                return null;

            return new AccountRef(model);
        }
    }
}

