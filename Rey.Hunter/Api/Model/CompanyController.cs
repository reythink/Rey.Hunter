using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Web.Business;
using System;
using System.Linq;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/")]
    public class CompanyController : ReyModelController<Company, string> {
        public CompanyController() {
            this.SearchQuery = (query, search) => {
                query = query.Where(x => x.Account.Id == this.CurrentAccount().Id);
                if (!string.IsNullOrEmpty(search)) {
                    query = query.Where(x => x.Name.Contains(search));
                }
                return query;
            };

            this.Create = (model) => {
                if (string.IsNullOrEmpty(model.Name))
                    throw new InvalidOperationException("Name cannot be empty");

                this.AttachCurrentAccount(model);
            };

            this.Update = (model, builder) => {
                if (string.IsNullOrEmpty(model.Name))
                    throw new InvalidOperationException("Name cannot be empty");

                return builder.Set(x => x.Name, model.Name)
                .Set(x => x.Industries, model.Industries)
                .Set(x => x.Type, model.Type)
                .Set(x => x.Status, model.Status)
                .Set(x => x.Contacts, model.Contacts)
                .Set(x => x.DepartmentStructures, model.DepartmentStructures)
                .Set(x => x.NameList, model.NameList)
                .Set(x => x.Introduction, model.Introduction)
                .Set(x => x.SalaryStructure, model.SalaryStructure)
                .Set(x => x.Culture, model.Culture)
                .Set(x => x.BasicRecruitmentPrinciple, model.BasicRecruitmentPrinciple);
            };
        }
    }
}
