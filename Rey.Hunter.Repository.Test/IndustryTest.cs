using Rey.Hunter.Models2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Rey.Hunter.Repository.Test {
    public class IndustryTest : TestBase {
        [Fact(DisplayName = "Industry.Actions")]
        public void TestIndustry() {
            var rep = this.Repository.Industry(this.Account);
            var model = new Industry { Name = "Consumer" };
            rep.InsertOne(model);
            Assert.NotNull(model.Id);
            Assert.NotNull(model.Account);
            Assert.Equal(model.Account.Id, this.Account.Id);

            rep.DeleteOne(model.Id);
        }

        [Fact(DisplayName = "Industry.Path")]
        public void Path() {
            var rep = this.Repository.Industry(this.Account);
            var random = new Random();
            var selected = this.Repository.Industry(this.Account)
                .FindAll()
                .Where(x => random.Next() % 2 == 0)
                .ToList();
            foreach (var node in selected) {
                var nodes = rep.Path(node).Select(x => x.Id).ToList();
                Assert.True(nodes.Contains(node.Id));

                if (node.Parent != null) {
                    Assert.True(nodes.Contains(node.Parent.Id));
                }
            }
        }
    }
}
