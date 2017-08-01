using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;
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

        [Fact(DisplayName = "Project.UpdateRef")]
        public void UpdateRef() {
            var repAccount = this.Repository.Account();

            var account = new Account { Company = "Test Account" };
            repAccount.InsertOne(account);
            Assert.NotNull(account.Id);

            var repProject = this.Repository.Project(account);
            var repCompany = this.Repository.Company(account);
            var repUser = this.Repository.User(account);
            var repFunction = this.Repository.Function(account);
            var repLocation = this.Repository.Location(account);
            var repTalent = this.Repository.Talent(account);

            var company = new Company { Name = "Test Company" };
            repCompany.InsertOne(company);
            Assert.NotNull(company.Id);

            var manager = new User { Name = "Manager" };
            var consultant = new User { Name = "Consultant" };
            repUser.InsertMany(manager, consultant);
            Assert.NotNull(manager.Id);
            Assert.NotNull(consultant.Id);

            var function = new Function { Name = "Test Function" };
            repFunction.InsertOne(function);
            Assert.NotNull(function.Id);

            var location = new Location { Name = "Test Location" };
            repLocation.InsertOne(location);
            Assert.NotNull(location.Id);

            var talent = new Talent { EnglishName = "Test Talent" };
            repTalent.InsertOne(talent);
            Assert.NotNull(talent.Id);

            var project = new Project { Position = "Test Project" };
            project.Client = company;
            project.Manager = manager;
            project.Consultant.Add(consultant);
            project.Function.Add(function);
            project.Location.Add(location);
            project.Candidate.Add(new ProjectCandidate { Talent = talent });
            repProject.InsertOne(project);
            Assert.NotNull(project.Id);

            repProject.UpdateRef(project);
            Assert.Null(project.Account.UpdateAt);
            Assert.Null(project.Client.UpdateAt);
            Assert.Null(project.Manager.UpdateAt);
            Assert.Null(project.Consultant.First().UpdateAt);
            Assert.Null(project.Function.First().UpdateAt);
            Assert.Null(project.Location.First().UpdateAt);
            Assert.Null(project.Candidate.First().Talent.UpdateAt);

            Assert.Null(account.ModifyAt);
            repAccount.ReplaceOne(account);
            Assert.NotNull(account.ModifyAt);

            repProject.UpdateRef(project);
            Assert.NotNull(project.Account.UpdateAt);
            Assert.True(project.Account.UpdateAt >= account.ModifyAt);

            Assert.Null(company.ModifyAt);
            repCompany.ReplaceOne(company);
            Assert.NotNull(company.ModifyAt);

            repProject.UpdateRef(project);
            Assert.NotNull(project.Client.UpdateAt);
            Assert.True(project.Client.UpdateAt >= company.ModifyAt);

            Assert.Null(manager.ModifyAt);
            repUser.ReplaceOne(manager);
            Assert.NotNull(manager.ModifyAt);

            repProject.UpdateRef(project);
            Assert.NotNull(project.Manager.UpdateAt);
            Assert.True(project.Manager.UpdateAt >= manager.ModifyAt);

            Assert.Null(consultant.ModifyAt);
            repUser.ReplaceOne(consultant);
            Assert.NotNull(consultant.ModifyAt);

            repProject.UpdateRef(project);
            Assert.NotNull(project.Consultant.First().UpdateAt);
            Assert.True(project.Consultant.First().UpdateAt >= consultant.ModifyAt);

            Assert.Null(function.ModifyAt);
            repFunction.ReplaceOne(function);
            Assert.NotNull(function.ModifyAt);

            repProject.UpdateRef(project);
            Assert.NotNull(project.Function.First().UpdateAt);
            Assert.True(project.Function.First().UpdateAt >= function.ModifyAt);

            Assert.Null(location.ModifyAt);
            repLocation.ReplaceOne(location);
            Assert.NotNull(location.ModifyAt);

            repProject.UpdateRef(project);
            Assert.NotNull(project.Location.First().UpdateAt);
            Assert.True(project.Location.First().UpdateAt >= location.ModifyAt);

            Assert.Null(talent.ModifyAt);
            talent.EnglishName = "Test Talent Changed";
            repTalent.ReplaceOne(talent);
            Assert.NotNull(talent.ModifyAt);

            repProject.UpdateRef(project);
            Assert.NotNull(project.Candidate.First().Talent.UpdateAt);
            Assert.True(project.Candidate.First().Talent.UpdateAt >= talent.ModifyAt);
            Assert.Equal(project.Candidate.First().Talent.EnglishName, talent.EnglishName);

            repCompany.DeleteOne(company.Id);
            repUser.DeleteMany(manager.Id, consultant.Id);
            repFunction.DeleteOne(function.Id);
            repLocation.DeleteOne(location.Id);
            repTalent.DeleteOne(talent.Id);
            repProject.DeleteOne(project.Id);
            repAccount.DeleteOne(account.Id);
        }
    }
}
