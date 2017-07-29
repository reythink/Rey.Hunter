using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public class TalentTest : TestBase {
        [Fact(DisplayName = "Talent.Query.CompanyName")]
        public void QueryCompanyName() {
            var rep = this.Repository.Talent(this.Account);
            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterCompanyName("p", "a")
                .Build(ret => result = ret)) {
                var exp = item.Experience.Where(x => x.Current ?? false).Single();
                var index1 = exp.Company.Name.IndexOf("p", StringComparison.CurrentCultureIgnoreCase);
                var index2 = exp.Company.Name.IndexOf("a", StringComparison.CurrentCultureIgnoreCase);
                Assert.True(index1 > -1 || index2 > -1);
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.PreviousCompanyName")]
        public void QueryPreviousCompanyName() {
            var rep = this.Repository.Talent(this.Account);
            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterPreviousCompanyName("p", "a")
                .Build(ret => result = ret)) {
                Assert.True(item.Experience.Where(x => !x.Current ?? true).Any(exp => {
                    var index1 = exp.Company.Name.IndexOf("p", StringComparison.CurrentCultureIgnoreCase);
                    var index2 = exp.Company.Name.IndexOf("a", StringComparison.CurrentCultureIgnoreCase);
                    return index1 > -1 || index2 > -1;
                }));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.Title")]
        public void QueryTitle() {
            var rep = this.Repository.Talent(this.Account);
            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterTitle("p", "a")
                .Build(ret => result = ret)) {
                var exp = item.Experience.Where(x => x.Current ?? false).Single();
                var index1 = exp.Title.IndexOf("p", StringComparison.CurrentCultureIgnoreCase);
                var index2 = exp.Title.IndexOf("a", StringComparison.CurrentCultureIgnoreCase);
                Assert.True(index1 > -1 || index2 > -1);
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.Responsibility")]
        public void QueryResponsibility() {
            var rep = this.Repository.Talent(this.Account);
            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterResponsibility("p", "a")
                .Build(ret => result = ret)) {
                var exp = item.Experience.Where(x => x.Current ?? false).Single();
                var index1 = exp.Responsibility.IndexOf("p", StringComparison.CurrentCultureIgnoreCase);
                var index2 = exp.Responsibility.IndexOf("a", StringComparison.CurrentCultureIgnoreCase);
                Assert.True(index1 > -1 || index2 > -1);
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.Grade")]
        public void QueryGrade() {
            var rep = this.Repository.Talent(this.Account);
            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterGrade("p", "a")
                .Build(ret => result = ret)) {
                var exp = item.Experience.Where(x => x.Current ?? false).Single();
                var index1 = exp.Grade.IndexOf("p", StringComparison.CurrentCultureIgnoreCase);
                var index2 = exp.Grade.IndexOf("a", StringComparison.CurrentCultureIgnoreCase);
                Assert.True(index1 > -1 || index2 > -1);
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.Industry")]
        public void QueryIndustry() {
            var rep = this.Repository.Talent(this.Account);
            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterIndustry("p", "a")
                .Build(ret => result = ret)) {
                Assert.True(item.Industry.Any(sub => {
                    var index1 = sub.Name.IndexOf("p", StringComparison.CurrentCultureIgnoreCase);
                    var index2 = sub.Name.IndexOf("a", StringComparison.CurrentCultureIgnoreCase);
                    return index1 > -1 || index2 > -1;
                }));
            }
            Assert.NotNull(result);
        }
    }
}
