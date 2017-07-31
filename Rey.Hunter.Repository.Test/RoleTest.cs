using Rey.Hunter.Models2;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public class RoleTest : TestBase {
        [Fact(DisplayName = "Role.Actions")]
        public void Actions() {
            var rep = this.Repository.Role(this.Account);
            var model = new Role() { Name = "Name" };

            Assert.Null(model.Id);

            rep.InsertOne(model);

            Assert.NotNull(model.Id);
            Assert.NotNull(model.Account);

            var found = rep.FindOne(model.Id);

            Assert.NotNull(found);
            Assert.NotNull(found.Id);
            Assert.NotNull(found.Account);

            Assert.Equal(found.Id, model.Id);
            Assert.Equal(found.Name, "Name");

            found.Name = "Name Changed";
            rep.ReplaceOne(found);

            found = rep.FindOne(found.Id);

            Assert.NotNull(found);
            Assert.NotNull(found.Id);
            Assert.NotNull(found.Account);

            Assert.Equal(found.Id, model.Id);
            Assert.Equal(found.Name, "Name Changed");

            rep.DeleteOne(found.Id);
            Assert.Null(rep.FindOne(found.Id));
        }
    }
}
