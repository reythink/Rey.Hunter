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
            Assert.Equal(rep.FindOne(model.Id).Name, model.Name);

            model.Name = "Role Name Changed";
            rep.ReplaceOne(model);
            Assert.Equal(rep.FindOne(model.Id).Name, model.Name);

            rep.DeleteOne(model.Id);
            Assert.Null(rep.FindOne(model.Id));
        }
    }
}
