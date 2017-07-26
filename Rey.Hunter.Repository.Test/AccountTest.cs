using Rey.Hunter.Models2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public class AccountTest {
        [Fact(DisplayName = "Account: Insert, Replace, Delete, Drop")]
        public void Test() {
            var mgr = new RepositoryManager();
            var repAccount = mgr.Account();


            var modelAccount = new Account() {
                Company = "Company1"
            };
            repAccount.InsertOne(modelAccount);

            modelAccount.Company = "Company2";
            repAccount.ReplaceOne(modelAccount);

            repAccount.DeleteOne(modelAccount.Id);

            modelAccount = new Account() {
                Company = "Company1"
            };
            repAccount.InsertOne(modelAccount);

            var repRole = mgr.Role(modelAccount.Id);

            var modelRole = new Role() {
                Name = "Role1",
                Account = modelAccount
            };

            repRole.InsertOne(modelRole);
            var roles = repRole.FindAll().ToList();
            foreach (var role in roles) {
                Assert.NotNull(role.Account.Id);
            }

            mgr.Client.GetDatabase("rey_hunter");

        }
    }
}
