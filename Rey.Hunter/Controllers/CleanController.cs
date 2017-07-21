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
                dynamic result = new ExpandoObject();

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
