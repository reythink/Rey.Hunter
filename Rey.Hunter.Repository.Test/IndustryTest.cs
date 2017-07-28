using Rey.Hunter.Models2.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public class IndustryTest : TestBase {
        [Fact(DisplayName = "TestIndustry")]
        public void TestIndustry() {
            var rep = this.Repository.Industry(this.Account);
            var model = new Industry { Name = "Consumer" };
            rep.InsertOne(model);
            Assert.NotNull(model.Id);
            Assert.NotNull(model.Account);
            Assert.Equal(model.Account.Id, this.Account.Id);

            rep.DeleteOne(model.Id);
        }
    }
}
