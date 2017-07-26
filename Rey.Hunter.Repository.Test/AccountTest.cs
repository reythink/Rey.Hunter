using Rey.Hunter.Models2;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public class AccountTest {
        [Fact]
        public void Test() {
            var mgr = new RepositoryManager();
            var model = new Account() {
                Company = "Reythink",
                Enabled = true
            };
            mgr.Account().InsertOne(model);
        }
    }
}
