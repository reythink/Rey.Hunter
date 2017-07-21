using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rey.Hunter.Models.Basic;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Dynamic;
using Rey.Hunter.Models.Business;
using Rey.Hunter.Models.Identity;
using Rey.Hunter.Models;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class CleanController : ReyController {
        public IActionResult Index() {
            this.ViewBag.Account = this.CurrentAccount();
            return View();
        }

        [HttpPost]
        public Task<IActionResult> Index(string clean) {
            return this.JsonInvokeOneAsync(() => {
                var account = this.CurrentAccount();
                dynamic result = new ExpandoObject();

                result.Category = this.GetMonCollection<CategoryNode>().MongoCollection.DeleteMany(x => !x.Account.Id.Equals(account.Id));
                result.Channel = this.GetMonCollection<ChannelNode>().MongoCollection.DeleteMany(x => !x.Account.Id.Equals(account.Id));
                result.Function = this.GetMonCollection<FunctionNode>().MongoCollection.DeleteMany(x => !x.Account.Id.Equals(account.Id));
                result.Industry = this.GetMonCollection<IndustryNode>().MongoCollection.DeleteMany(x => !x.Account.Id.Equals(account.Id));
                result.Location = this.GetMonCollection<LocationNode>().MongoCollection.DeleteMany(x => !x.Account.Id.Equals(account.Id));

                result.Company = this.GetMonCollection<Company>().MongoCollection.DeleteMany(x => !x.Account.Id.Equals(account.Id));
                result.Talent = this.GetMonCollection<Talent>().MongoCollection.DeleteMany(x => !x.Account.Id.Equals(account.Id));
                result.Project = this.GetMonCollection<Project>().MongoCollection.DeleteMany(x => !x.Account.Id.Equals(account.Id));

                result.Account = this.GetMonCollection<Account>().MongoCollection.DeleteMany(x => !x.Id.Equals(account.Id));
                result.Role = this.GetMonCollection<Role>().MongoCollection.DeleteMany(x => !x.Account.Id.Equals(account.Id));
                result.User = this.GetMonCollection<User>().MongoCollection.DeleteMany(x => !x.Account.Id.Equals(account.Id));

                result.CategoryReplace = ReplaceModels<CategoryNode>();
                result.ChannelReplace = ReplaceModels<ChannelNode>();
                result.FunctionReplace = ReplaceModels<FunctionNode>();
                result.IndustryReplace = ReplaceModels<IndustryNode>();
                result.LocationReplace = ReplaceModels<LocationNode>();

                result.CompanyReplace = ReplaceModels<Company>();
                result.TalentReplace = ReplaceModels<Talent>();
                result.ProjectReplace = ReplaceModels<Project>();

                result.AccountReplace = ReplaceModels<Account>();
                result.RoleReplace = ReplaceModels<Role>();
                result.UserReplace = ReplaceModels<User>();

                return result;
            });
        }

        private long ReplaceModels<TModel>() where TModel : Model {
            var collection = this.GetMonCollection<TModel>();
            var models = collection.FindMany(x => true);
            long count = 0;
            foreach (var model in models) {
                count += collection.MongoCollection.ReplaceOne(x => x.Id == model.Id, model).ModifiedCount;
            }
            return count;
        }

        private UpdateResult RemoveAccountType<TModel>() {
            return this.GetMonCollection<TModel>().MongoCollection.UpdateMany(Builders<TModel>.Filter.Exists("Account._t"), Builders<TModel>.Update.Unset("Account._t"));
        }
    }
}
