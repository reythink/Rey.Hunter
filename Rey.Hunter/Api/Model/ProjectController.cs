using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/")]
    public class ProjectController : ReyAccountModelController<Project> {
        public ProjectController() {
            this.BeforeSearch += (query, search) => {
                return query.Where(x =>
                x.Name.ToLower().Contains(search.ToLower())
                );
            };
        }
    }
}
