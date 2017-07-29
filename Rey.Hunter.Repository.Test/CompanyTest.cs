using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;
using Rey.Hunter.Models2.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public class CompanyTest : TestBase {
        [Fact(DisplayName = "TestCompany")]
        public void TestCompany() {
            var rep = this.Repository.Company(this.Account);
            var model = new Company() {
                Name = "Company Name"
            };

            rep.InsertOne(model);
            Assert.NotNull(model.Id);
            Assert.NotNull(model.Account);

            var found = rep.FindOne(model.Id);
            Assert.NotNull(found);
            Assert.NotNull(found.Id);
            Assert.NotNull(found.Account);
            Assert.Equal(found.Name, "Company Name");

            model.Name = "Company Name Changed";
            rep.ReplaceOne(model);
            found = rep.FindOne(model.Id);
            Assert.NotNull(found);
            Assert.NotNull(found.Id);
            Assert.NotNull(found.Account);
            Assert.Equal(found.Name, "Company Name Changed");

            rep.DeleteOne(model.Id);
            Assert.Null(rep.FindOne(model.Id));
        }

        [Fact(DisplayName = "Company.Query.Name")]
        public void QueryName() {
            var rep = this.Repository.Company(this.Account);
            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterName("p", "a")
                .Build(ret => result = ret)) {
                var index1 = item.Name.IndexOf("p", StringComparison.CurrentCultureIgnoreCase);
                var index2 = item.Name.IndexOf("a", StringComparison.CurrentCultureIgnoreCase);
                Assert.True(index1 > -1 || index2 > -1);
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Company.Query.Industry")]
        public void QueryIndustry() {
            var rep = this.Repository.Company(this.Account);
            var random = new Random();
            var selected = this.Repository.Industry(this.Account)
                .FindAll()
                .Where(x => random.Next() % 2 == 0)
                .Select(x => x.Id)
                .ToList();

            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterIndustry(selected.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(item.Industry.Any(sub => selected.Contains(sub.Id)));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Company.Query.Type")]
        public void QueryType() {
            var rep = this.Repository.Company(this.Account);
            var values = Enum.GetValues(typeof(CompanyType)).Cast<CompanyType>();
            QueryResult result = null;

            foreach (var value in values) {
                foreach (var item in rep.Query()
                    .FilterType(value)
                    .Build(ret => result = ret)) {
                    Assert.True(item.Type == value);
                }
                Assert.NotNull(result);
            }

            foreach (var item in rep.Query()
                .FilterType(values.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(values.Contains(item.Type.Value));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Company.Query.Status")]
        public void QueryStatus() {
            var rep = this.Repository.Company(this.Account);
            var values = Enum.GetValues(typeof(CompanyStatus)).Cast<CompanyStatus>();
            QueryResult result = null;

            foreach (var value in values) {
                foreach (var item in rep.Query()
                    .FilterStatus(value)
                    .Build(ret => result = ret)) {
                    Assert.True(item.Status == value);
                }
                Assert.NotNull(result);
            }

            foreach (var item in rep.Query()
                .FilterStatus(values.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(values.Contains(item.Status.Value));
            }
            Assert.NotNull(result);
        }
    }
}
