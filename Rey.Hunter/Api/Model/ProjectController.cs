using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/")]
    public class ProjectController : ReyModelController<Project, string> {
        public ProjectController() {
            this.Query = (query) => query.Where(x => x.Account.Id == this.CurrentAccount().Id);

            this.Create = (model) => {
                this.AttachCurrentAccount(model);
            };

            this.Update = (model, builder) => {
                return builder.Set(x => x.Name, model.Name)
                .Set(x => x.Headcount, model.Headcount)
                .Set(x => x.Client, model.Client)
                .Set(x => x.Functions, model.Functions)
                .Set(x => x.Locations, model.Locations)
                .Set(x => x.StartDate, model.StartDate)
                .Set(x => x.AssignmentDate, model.AssignmentDate)
                .Set(x => x.OnBoardDate, model.OnBoardDate)
                .Set(x => x.Notes, model.Notes)
                .Set(x => x.Candidates, model.Candidates);
            };
        }
    }
}
