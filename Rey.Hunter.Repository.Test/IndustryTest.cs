using Rey.Hunter.Models2.Basic;
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
            Assert.NotNull(model.Account);
            Assert.NotNull(model.Account.Id);
            Assert.Equal(model.Account.Id, this.AccountId);

            rep.DeleteOne(model.Id);
        }

        [Fact(DisplayName = "InitIndustry")]
        public void InitIndustry() {
            var rep = this.Repository.Industry(this.AccountId);
            rep.Drop();
            rep.Initialize();
        }
    }
}
