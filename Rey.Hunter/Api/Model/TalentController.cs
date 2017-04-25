using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Business;
using System;
using System.Linq;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/")]
    public class TalentController : ReyAccountModelController<Talent> {
        public TalentController() {
            this.BeforeSearch += (query, search) => {
                return query.Where(x =>
                x.EnglishName.ToLower().Contains(search.ToLower()) ||
                x.ChineseName.ToLower().Contains(search.ToLower()) ||
                x.Email.Contains(search) ||
                x.Mobile.Contains(search) ||
                x.Phone.Contains(search)
                );
            };
        }
    }
}
