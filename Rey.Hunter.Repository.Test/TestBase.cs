using Rey.Hunter.Models2;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public abstract class TestBase {
        protected IRepositoryManager Repository { get; } = new RepositoryManager();
        private string AccountId { get; } = "58ff2e23a31baa1d28b77fd0";
        protected Account Account { get; }

        public TestBase() {
            this.Account = this.Repository.Account().FindOne(this.AccountId);
            if (this.Account == null) {
                this.Account = new Account() { Id = this.AccountId, Company = "Reythink" };
                this.Repository.Account().InsertOne(this.Account);
            }
        }
    }
}
