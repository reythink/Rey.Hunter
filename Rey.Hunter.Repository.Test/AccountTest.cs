using Rey.Hunter.Models2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public class AccountTest : TestBase {
        [Fact(DisplayName = "Account.Actions")]
        public void Actions() {
            var rep = this.Repository.Account();
            var model1 = new Account() { Company = "Company1" };
            var model2 = new Account() { Company = "Company2", Enabled = false };

            Assert.Null(model1.Id);
            Assert.Null(model2.Id);

            rep.InsertOne(model1);
            rep.InsertOne(model2);

            Assert.NotNull(model1.Id);
            Assert.NotNull(model2.Id);

            var found1 = rep.FindOne(model1.Id);
            var found2 = rep.FindOne(model2.Id);

            Assert.Equal(found1.Id, model1.Id);
            Assert.Equal(found1.Company, "Company1");
            Assert.True(found1.Enabled);

            Assert.Equal(found2.Id, model2.Id);
            Assert.Equal(found2.Company, "Company2");
            Assert.False(found2.Enabled);

            found1.Company = "Company1 Changed";
            found1.Enabled = false;

            found2.Company = "Company2 Changed";
            found2.Enabled = true;

            rep.ReplaceOne(found1);
            rep.ReplaceOne(found2);

            found1 = rep.FindOne(found1.Id);
            found2 = rep.FindOne(found2.Id);

            Assert.Equal(found1.Id, model1.Id);
            Assert.Equal(found1.Company, "Company1 Changed");
            Assert.False(found1.Enabled);
            Assert.NotNull(found1.ModifyAt);

            Assert.Equal(found2.Id, model2.Id);
            Assert.Equal(found2.Company, "Company2 Changed");
            Assert.True(found2.Enabled);
            Assert.NotNull(found2.ModifyAt);

            rep.DeleteOne(found1.Id);
            rep.DeleteOne(found2.Id);

            Assert.Null(rep.FindOne(found1.Id));
            Assert.Null(rep.FindOne(found2.Id));
        }
    }
}
