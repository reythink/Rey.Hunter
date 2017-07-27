using Rey.Hunter.Models2;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public class RoleTest : TestBase {
        [Fact(DisplayName = "TestRole")]
        public void TestRole() {
            var rep = this.Repository.Role(this.AccountId);
            var model = new Role() {
                Name = "Role Name"
            };

            rep.InsertOne(model);
            Assert.NotNull(model.Id);
            Assert.NotNull(model.Account);
            Assert.NotNull(model.Account.Key);

            var found = rep.FindOne(model.Id);
            Assert.NotNull(found);
            Assert.NotNull(found.Id);
            Assert.NotNull(found.Account);
            Assert.NotNull(found.Account.Key);
            Assert.Equal(found.Name, "Role Name");

            model.Name = "Role Name Changed";
            rep.ReplaceOne(model);
            found = rep.FindOne(model.Id);
            Assert.NotNull(found);
            Assert.NotNull(found.Id);
            Assert.NotNull(found.Account);
            Assert.NotNull(found.Account.Key);
            Assert.Equal(found.Name, "Role Name Changed");

            rep.DeleteOne(model.Id);
            Assert.Null(rep.FindOne(model.Id));
        }
    }
}
