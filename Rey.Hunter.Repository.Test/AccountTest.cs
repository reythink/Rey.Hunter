using Rey.Hunter.Models2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public class AccountTest : TestBase {
        [Fact(DisplayName = "TestAccount")]
        public void TestAccount() {
            var rep = this.Repository.Account();
            var model = new Account() {
                Company = "Company1"
            };

            rep.InsertOne(model);
            Assert.Equal(rep.FindOne(model.Id).Company, model.Company);

            model.Company = "Company2";
            rep.ReplaceOne(model);
            Assert.Equal(rep.FindOne(model.Id).Company, model.Company);

            rep.DeleteOne(model.Id);
            Assert.Null(rep.FindOne(model.Id));
        }

        [Fact(DisplayName = "TestAccountQuery")]
        public void TestAccountQuery() {
            //var rep = this.Repository.Account();
            //var models = new List<Account>();
            //for (var i = 0; i < 100000; ++i) {
            //    models.Add(new Account { Company = $"Company{i.ToString("00000")}" });
            //}
            //rep.InsertMany(models);

            //foreach (var model in models) {
            //    Assert.NotNull(model.Id);
            //}

            //rep.DeleteMany(models.Select(x => x.Id));
        }
    }
}
