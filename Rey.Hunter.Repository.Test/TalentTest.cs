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
                Assert.True(item.Experience.Where(x => !x.Current ?? true).Any(exp =>
                    exp.Company.Name.IndexOf("p", StringComparison.CurrentCultureIgnoreCase) > -1 ||
                    exp.Company.Name.IndexOf("a", StringComparison.CurrentCultureIgnoreCase) > -1
                ));
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

        [Fact(DisplayName = "Talent.Query.CrossIndustry")]
        public void QueryCrossIndustry() {
            var rep = this.Repository.Talent(this.Account);
            var random = new Random();
            var selected = this.Repository.Industry(this.Account)
                .FindAll()
                .Where(x => random.Next() % 2 == 0)
                .Select(x => x.Id)
                .ToList();

            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterCrossIndustry(selected.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(item.Profile.CrossIndustry.Any(sub => selected.Contains(sub.Id)));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.Function")]
        public void QueryFunction() {
            var rep = this.Repository.Talent(this.Account);
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

        [Fact(DisplayName = "Talent.Query.CrossFunction")]
        public void QueryCrossFunction() {
            var rep = this.Repository.Talent(this.Account);
            var random = new Random();
            var selected = this.Repository.Function(this.Account)
                .FindAll()
                .Where(x => random.Next() % 2 == 0)
                .Select(x => x.Id)
                .ToList();

            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterCrossFunction(selected.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(item.Profile.CrossFunction.Any(sub => selected.Contains(sub.Id)));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.CrossCategory")]
        public void QueryCrossCategory() {
            var rep = this.Repository.Talent(this.Account);
            var random = new Random();
            var selected = this.Repository.Category(this.Account)
                .FindAll()
                .Where(x => random.Next() % 2 == 0)
                .Select(x => x.Id)
                .ToList();

            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterCrossCategory(selected.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(item.Profile.CrossCategory.Any(sub => selected.Contains(sub.Id)));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.CrossChannel")]
        public void QueryCrossChannel() {
            var rep = this.Repository.Talent(this.Account);
            var random = new Random();
            var selected = this.Repository.Channel(this.Account)
                .FindAll()
                .Where(x => random.Next() % 2 == 0)
                .Select(x => x.Id)
                .ToList();

            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterCrossChannel(selected.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(item.Profile.CrossChannel.Any(sub => selected.Contains(sub.Id)));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.Brand")]
        public void QueryBrand() {
            var rep = this.Repository.Talent(this.Account);
            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterBrand("p", "a")
                .Build(ret => result = ret)) {
                var index1 = item.Profile.Brand.IndexOf("p", StringComparison.CurrentCultureIgnoreCase);
                var index2 = item.Profile.Brand.IndexOf("a", StringComparison.CurrentCultureIgnoreCase);
                Assert.True(index1 > -1 || index2 > -1);
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.KeyAccount")]
        public void QueryKeyAccount() {
            var rep = this.Repository.Talent(this.Account);
            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterKeyAccount("p", "a")
                .Build(ret => result = ret)) {
                var index1 = item.Profile.KeyAccount.IndexOf("p", StringComparison.CurrentCultureIgnoreCase);
                var index2 = item.Profile.KeyAccount.IndexOf("a", StringComparison.CurrentCultureIgnoreCase);
                Assert.True(index1 > -1 || index2 > -1);
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.CurrentLocation")]
        public void QueryCurrentLocation() {
            var rep = this.Repository.Talent(this.Account);
            var random = new Random();
            var selected = this.Repository.Location(this.Account)
                .FindAll()
                .Where(x => random.Next() % 2 == 0)
                .Select(x => x.Id)
                .ToList();

            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterCurrentLocation(selected.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(selected.Contains(item.Location.Current.Id));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.MobilityLocation")]
        public void QueryMobilityLocation() {
            var rep = this.Repository.Talent(this.Account);
            var random = new Random();
            var selected = this.Repository.Location(this.Account)
                .FindAll()
                .Where(x => random.Next() % 2 == 0)
                .Select(x => x.Id)
                .ToList();

            QueryResult result = null;
            foreach (var item in rep.Query()
                .FilterMobilityLocation(selected.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(item.Location.Mobility.Any(sub => selected.Contains(sub.Id)));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.Gender")]
        public void QueryGender() {
            var rep = this.Repository.Talent(this.Account);
            var values = Enum.GetValues(typeof(Gender)).Cast<Gender>();
            QueryResult result = null;

            foreach (var value in values) {
                foreach (var item in rep.Query()
                    .FilterGender(value)
                    .Build(ret => result = ret)) {
                    Assert.True(item.Gender == value);
                }
                Assert.NotNull(result);
            }

            foreach (var item in rep.Query()
                .FilterGender(values.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(values.Contains(item.Gender.Value));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.Education")]
        public void QueryEducation() {
            var rep = this.Repository.Talent(this.Account);
            var values = Enum.GetValues(typeof(Education)).Cast<Education>();
            QueryResult result = null;

            foreach (var value in values) {
                foreach (var item in rep.Query()
                    .FilterEducation(value)
                    .Build(ret => result = ret)) {
                    Assert.True(item.Education == value);
                }
                Assert.NotNull(result);
            }

            foreach (var item in rep.Query()
                .FilterEducation(values.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(values.Contains(item.Education.Value));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.Language")]
        public void QueryLanguage() {
            var rep = this.Repository.Talent(this.Account);
            var values = Enum.GetValues(typeof(Language)).Cast<Language>();
            QueryResult result = null;

            foreach (var value in values) {
                foreach (var item in rep.Query()
                    .FilterLanguage(value)
                    .Build(ret => result = ret)) {
                    Assert.True(item.Language == value);
                }
                Assert.NotNull(result);
            }

            foreach (var item in rep.Query()
                .FilterLanguage(values.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(values.Contains(item.Language.Value));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.Nationality")]
        public void QueryNationality() {
            var rep = this.Repository.Talent(this.Account);
            var values = Enum.GetValues(typeof(Nationality)).Cast<Nationality>();
            QueryResult result = null;

            foreach (var value in values) {
                foreach (var item in rep.Query()
                    .FilterNationality(value)
                    .Build(ret => result = ret)) {
                    Assert.True(item.Nationality == value);
                }
                Assert.NotNull(result);
            }

            foreach (var item in rep.Query()
                .FilterNationality(values.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(values.Contains(item.Nationality.Value));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.Query.Intension")]
        public void QueryIntension() {
            var rep = this.Repository.Talent(this.Account);
            var values = Enum.GetValues(typeof(Intension)).Cast<Intension>();
            QueryResult result = null;

            foreach (var value in values) {
                foreach (var item in rep.Query()
                    .FilterIntension(value)
                    .Build(ret => result = ret)) {
                    Assert.True(item.Intension == value);
                }
                Assert.NotNull(result);
            }

            foreach (var item in rep.Query()
                .FilterIntension(values.ToArray())
                .Build(ret => result = ret)) {
                Assert.True(values.Contains(item.Intension.Value));
            }
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Talent.UpdateRef")]
        public void UpdateRef() {
            var repAccount = this.Repository.Account();

            var account = new Account { Company = "Test Account" };
            repAccount.InsertOne(account);
            Assert.NotNull(account.Id);

            var repTalent = this.Repository.Talent(account);
            var repIndustry = this.Repository.Industry(account);
            var repFunction = this.Repository.Function(account);
            var repLocation = this.Repository.Location(account);
            var repCategory = this.Repository.Category(account);
            var repChannel = this.Repository.Channel(account);

            var industry = new Industry { Name = "Test Industry" };
            repIndustry.InsertOne(industry);
            Assert.NotNull(industry.Id);

            var function = new Function { Name = "Test Function" };
            repFunction.InsertOne(function);
            Assert.NotNull(function.Id);

            var currentLocation = new Location { Name = "Shanghai" };
            var mobilityLocation = new Location { Name = "Beijing" };
            repLocation.InsertMany(currentLocation, mobilityLocation);
            Assert.NotNull(currentLocation.Id);
            Assert.NotNull(mobilityLocation.Id);

            var talent = new Talent { EnglishName = "Test Talent" };
            talent.Industry.Add(industry);
            talent.Function.Add(function);
            talent.Location.Current = currentLocation;
            talent.Location.Mobility.Add(mobilityLocation);


            repTalent.InsertOne(talent);
            Assert.NotNull(talent.Id);

            repTalent.UpdateRef(talent);
            Assert.Null(talent.Account.UpdateAt);
            Assert.Null(talent.Industry.First().UpdateAt);
            Assert.Null(talent.Function.First().UpdateAt);
            Assert.Null(talent.Location.Current.UpdateAt);
            Assert.Null(talent.Location.Mobility.First().UpdateAt);

            Assert.Null(account.ModifyAt);
            repAccount.ReplaceOne(account);
            Assert.NotNull(account.ModifyAt);

            repTalent.UpdateRef(talent);
            Assert.NotNull(talent.Account.UpdateAt);
            Assert.True(talent.Account.UpdateAt >= account.ModifyAt);

            Assert.Null(industry.ModifyAt);
            repIndustry.ReplaceOne(industry);
            Assert.NotNull(industry.ModifyAt);

            repTalent.UpdateRef(talent);
            Assert.NotNull(talent.Industry.First().UpdateAt);
            Assert.True(talent.Industry.First().UpdateAt >= industry.ModifyAt);

            Assert.Null(function.ModifyAt);
            repFunction.ReplaceOne(function);
            Assert.NotNull(function.ModifyAt);

            repTalent.UpdateRef(talent);
            Assert.NotNull(talent.Function.First().UpdateAt);
            Assert.True(talent.Function.First().UpdateAt >= function.ModifyAt);

            Assert.Null(currentLocation.ModifyAt);
            repLocation.ReplaceOne(currentLocation);
            Assert.NotNull(currentLocation.ModifyAt);

            repTalent.UpdateRef(talent);
            Assert.NotNull(talent.Location.Current.UpdateAt);
            Assert.True(talent.Location.Current.UpdateAt >= currentLocation.ModifyAt);

            Assert.Null(mobilityLocation.ModifyAt);
            repLocation.ReplaceOne(mobilityLocation);
            Assert.NotNull(mobilityLocation.ModifyAt);

            repTalent.UpdateRef(talent);
            Assert.NotNull(talent.Location.Mobility.First().UpdateAt);
            Assert.True(talent.Location.Mobility.First().UpdateAt >= mobilityLocation.ModifyAt);



            repIndustry.DeleteOne(industry.Id);
            repFunction.DeleteOne(function.Id);
            repLocation.DeleteMany(currentLocation.Id, mobilityLocation.Id);
            repTalent.DeleteOne(talent.Id);
            repAccount.DeleteOne(account.Id);
        }
    }
}
