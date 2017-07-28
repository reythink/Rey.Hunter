using Rey.Hunter.Models2.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public class IndustryTest : TestBase {
        [Fact(DisplayName = "TestIndustry")]
        public void TestIndustry() {
            var rep = this.Repository.Industry(this.AccountId);
            var model = new Industry { Name = "Consumer" };
            rep.InsertOne(model);
            Assert.NotNull(model.Id);
            Assert.NotNull(model.AccountId);
            Assert.Equal(model.AccountId, this.AccountId);

            rep.DeleteOne(model.Id);
        }
    }
}
