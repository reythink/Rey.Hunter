using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public class CompanyTest : TestBase {
        [Fact(DisplayName = "TestCompany")]
        public void TestCompany() {
            var rep = this.Repository.Company(this.AccountId);
            var model = new Company() {
                Name = "Company Name"
            };
            var industries = this.Repository.Industry(this.AccountId).FindAll();
            model.Industry.AddRange(industries.Select(x => (ModelRef<Industry>)x));

            rep.InsertOne(model);
            Assert.NotNull(model.Id);
            Assert.NotNull(model.Account);
            Assert.NotNull(model.Account.Key);

            var found = rep.FindOne(model.Id);
            Assert.NotNull(found);
            Assert.NotNull(found.Id);
            Assert.NotNull(found.Account);
            Assert.NotNull(found.Account.Key);
            Assert.Equal(found.Name, "Company Name");

            model.Name = "Company Name Changed";
            rep.ReplaceOne(model);
            found = rep.FindOne(model.Id);
            Assert.NotNull(found);
            Assert.NotNull(found.Id);
            Assert.NotNull(found.Account);
            Assert.NotNull(found.Account.Key);
            Assert.Equal(found.Name, "Company Name Changed");

            rep.DeleteOne(model.Id);
            Assert.Null(rep.FindOne(model.Id));
        }

        [Fact(DisplayName = "TestCompanyQuery")]
        public void TestCompanyQuery() {
            var rep = this.Repository.Company(this.AccountId);
            rep.Query();
        }

        [Fact(DisplayName = "InitCompany")]
        private void InitCompany() {
            var rep = this.Repository.Company(this.AccountId);
            rep.Drop();

            var industries = this.Repository.Industry(this.AccountId).FindAll().ToList();

            var models = new List<Company>();
            var rand = new Random();
            for (var i = 0; i < 100000; ++i) {
                var model = new Company {
                    Name = $"Company{i.ToString("00000")}",
                };
                model.Industry.AddRange(RandomIndustry(industries, rand));

                models.Add(model);
            }
            rep.InsertMany(models);
        }

        private List<ModelRef<Industry>> RandomIndustry(List<Industry> industry, Random rand) {
            var count = rand.Next(2, 5);
            var results = new List<ModelRef<Industry>>();
            for (var i = 0; i < count; ++i) {
                results.Add(industry[rand.Next(industry.Count)]);
            }
            return results;
        }
    }
}
