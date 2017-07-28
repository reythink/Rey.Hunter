using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;
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
            model.Industry.AddRange(industries.Select(x => x.Name));

            rep.InsertOne(model);
            Assert.NotNull(model.Id);
            Assert.NotNull(model.AccountId);

            var found = rep.FindOne(model.Id);
            Assert.NotNull(found);
            Assert.NotNull(found.Id);
            Assert.NotNull(found.AccountId);
            Assert.Equal(found.Name, "Company Name");

            model.Name = "Company Name Changed";
            rep.ReplaceOne(model);
            found = rep.FindOne(model.Id);
            Assert.NotNull(found);
            Assert.NotNull(found.Id);
            Assert.NotNull(found.AccountId);
            Assert.Equal(found.Name, "Company Name Changed");

            rep.DeleteOne(model.Id);
            Assert.Null(rep.FindOne(model.Id));
        }

        [Fact(DisplayName = "TestCompanyQuery")]
        public void TestCompanyQuery() {
            var rep = this.Repository.Company(this.AccountId);
            var list = rep.Query()
                //.Name("1")
                .Build()
                .ToList();
        }

        private List<string> RandomIndustry(List<Industry> industry, Random rand) {
            var count = rand.Next(2, 5);
            var results = new List<string>();
            for (var i = 0; i < count; ++i) {
                results.Add(industry[rand.Next(industry.Count)].Name);
            }
            return results;
        }
    }
}
