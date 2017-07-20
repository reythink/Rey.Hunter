using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Business;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet("Sample")]
        public Task<IActionResult> SampleQueryAction(string search) {
            return this.JsonInvokeManyAsync(() => {
                IQueryable<Company> query = this.Collection.Query();

                if (!string.IsNullOrEmpty(search)) {
                    query = query.Where(x => x.Name.ToLower().Contains(search.ToLower()));
                }

                return query.Select(x => (new { id = x.Id, Name = x.Name }));
            });
        }
    }
}
