using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Web.Business;
using System;
using System.Linq;

namespace Rey.Hunter.Api {
    [Route("/Api/[controller]/")]
    public class TalentController : ReyModelController<Talent, string> {
        public TalentController() {
            this.Query = (query) => query.Where(x => x.Account.Id == this.CurrentAccount().Id);

            this.Create = (model) => {
                this.AttachCurrentAccount(model);
            };

            this.Update = (model, builder) => {
                //! Basics
                return builder.Set(x => x.EnglishName, model.EnglishName)
                .Set(x => x.ChineseName, model.ChineseName)
                .Set(x => x.Birthday, model.Birthday)
                .Set(x => x.Gender, model.Gender)
                .Set(x => x.MaritalStatus, model.MaritalStatus)
                .Set(x => x.EducationLevel, model.EducationLevel)
                .Set(x => x.Language, model.Language)
                .Set(x => x.Nationality, model.Nationality)
                .Set(x => x.Intension, model.Intension)
                .Set(x => x.Linkedin, model.Linkedin)
                .Set(x => x.Notes, model.Notes)

                //! Locations
                .Set(x => x.CurrentLocations, model.CurrentLocations)
                .Set(x => x.MobilityLocations, model.MobilityLocations)

                //! Contacts
                .Set(x => x.Phone, model.Phone)
                .Set(x => x.Mobile, model.Mobile)
                .Set(x => x.Email, model.Email)
                .Set(x => x.QQ, model.QQ)
                .Set(x => x.Wechat, model.Wechat)

                //! Profile Label
                .Set(x => x.ProfileLabel, model.ProfileLabel)

                //! Work Experience
                .Set(x => x.Experiences, model.Experiences)
                ;
            };
        }
    }
}
