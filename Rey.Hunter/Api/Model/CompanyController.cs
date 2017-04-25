using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Business;
using System;
using System.Linq;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/")]
    public class CompanyController : ReyAccountModelController<Company> {
        public CompanyController() {
            this.BeforeSearch += (query, search) => {
                return query.Where(x =>
                x.Name.ToLower().Contains(search.ToLower())
                );
            };
        }
    }
}
