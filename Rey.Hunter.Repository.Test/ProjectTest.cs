using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public class ProjectTest : TestBase {
        [Fact(DisplayName = "Project.Query.Position")]
        public void QueryPosition() {
            var rep = this.Repository.Project(this.Account);
            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterPosition("p", "a")
                .Build(ret => result = ret)) {
                var index1 = item.Position.IndexOf("p", StringComparison.CurrentCultureIgnoreCase);
                var index2 = item.Position.IndexOf("a", StringComparison.CurrentCultureIgnoreCase);
                Assert.True(index1 > -1 || index2 > -1);
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Project.Query.ClientName")]
        public void QueryClientName() {
            var rep = this.Repository.Project(this.Account);
            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterClientName("p", "a")
                .Build(ret => result = ret)) {
                var index1 = item.Client.Name.IndexOf("p", StringComparison.CurrentCultureIgnoreCase);
                var index2 = item.Client.Name.IndexOf("a", StringComparison.CurrentCultureIgnoreCase);
                Assert.True(index1 > -1 || index2 > -1);
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Project.Query.Function")]
        public void QueryFunction() {
            var rep = this.Repository.Project(this.Account);
            var random = new Random();
            var selected = this.Repository.Function(this.Account)
                .FindAll()
                .Where(x => random.Next() % 2 == 0)
                .Select(x => x.Id)
                .ToList();

            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterFunction(selected.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(item.Function.Any(sub => selected.Contains(sub.Id)));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Project.Query.Manager")]
        public void QueryManager() {
            var rep = this.Repository.Project(this.Account);
            var users = this.Repository.User(this.Account).FindAll();
            QueryResult result = null;

            foreach (var user in users) {
                foreach (var item in rep.Query()
                    .FilterManager(user.Id)
                    .Build(ret => result = ret)) {
                    Assert.True(item.Manager.Id == user.Id);
                }
                Assert.NotNull(result);
            }
        }

        [Fact(DisplayName = "Project.Query.Consultant")]
        public void QueryConsultant() {
            var rep = this.Repository.Project(this.Account);
            var users = this.Repository.User(this.Account).FindAll();
            QueryResult result = null;

            foreach (var user in users) {
                foreach (var item in rep.Query()
                    .FilterConsultant(user.Id)
                    .Build(ret => result = ret)) {
                    Assert.True(item.Consultant.Select(x => x.Id).Contains(user.Id));
                }
                Assert.NotNull(result);
            }
        }
    }
}
