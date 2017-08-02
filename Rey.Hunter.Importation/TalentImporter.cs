using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;
using Rey.Hunter.Models2.Enums;
using System;
using System.Linq;

namespace Rey.Hunter.Importation {
    public class TalentImporter : ImporterBase<Talent>, IAccountImporter<Talent> {
        public TalentImporter(IImportManager manager)
            : base(manager) {
        }

        public void Import(Account account) {
            using (var tool = this.BeginImport()) {
                var items = tool.GetImportItems(account);
                foreach (var item in items) {
                    var model = tool.CreateModel();
                    model.Account = account;
                    model.Id = tool.GetValue<string>(item, "_id");
                    model.CreateAt = tool.GetValue<DateTime?>(item, "CreateAt");

                    model.Industry.AddRange(tool.FindMany<Industry>(tool.GetIdList(item, "Industries")).Select(x => (IndustryRef)x));
                    model.Function.AddRange(tool.FindMany<Function>(tool.GetIdList(item, "Functions")).Select(x => (FunctionRef)x));
                    model.EnglishName = (string)tool.GetValue(item, "EnglishName");
                    model.ChineseName = (string)tool.GetValue(item, "ChineseName");
                    model.BirthYear = (int?)tool.GetValue(item, "BirthYear");
                    model.Gender = (Gender?)(int?)tool.GetValue(item, "Gender");
                    model.Marital = (Marital?)(int?)tool.GetValue(item, "MaritalStatus");
                    model.Education = (Education?)(int?)tool.GetValue(item, "EducationLevel");
                    model.Language = (Language?)(int?)tool.GetValue(item, "Language");
                    model.Nationality = (Nationality?)(int?)tool.GetValue(item, "Nationality");
                    model.Intension = (Intension?)(int?)tool.GetValue(item, "Intension");
                    model.Linkedin = (string)tool.GetValue(item, "Linkedin");
                    model.Vita = (string)tool.GetValue(item, "CV");
                    model.Notes = (string)tool.GetValue(item, "Notes");

                    model.Location.Current = tool.FindOne<Location>(tool.GetIdList(item, "CurrentLocations").FirstOrDefault());
                    model.Location.Mobility.AddRange(tool.FindMany<Location>(tool.GetIdList(item, "MobilityLocations")).Select(x => (LocationRef)x));

                    model.Contact.Phone = (string)tool.GetValue(item, "Phone");
                    model.Contact.Mobile = (string)tool.GetValue(item, "Mobile");
                    model.Contact.Email = (string)tool.GetValue(item, "Email");
                    model.Contact.QQ = (string)tool.GetValue(item, "QQ");
                    model.Contact.Wechat = (string)tool.GetValue(item, "Wechat");

                    model.Profile.CrossIndustry.AddRange(tool.FindMany<Industry>(tool.GetIdList(item, "ProfileLabel.CrossIndustries")).Select(x => (IndustryRef)x));
                    model.Profile.CrossFunction.AddRange(tool.FindMany<Function>(tool.GetIdList(item, "ProfileLabel.CrossFunctions")).Select(x => (FunctionRef)x));
                    model.Profile.CrossChannel.AddRange(tool.FindMany<Channel>(tool.GetIdList(item, "ProfileLabel.CrossChannels")).Select(x => (ChannelRef)x));
                    model.Profile.CrossCategory.AddRange(tool.FindMany<Category>(tool.GetIdList(item, "ProfileLabel.CrossCategories")).Select(x => (CategoryRef)x));
                    model.Profile.Brand = (string)tool.GetValue(item, "ProfileLabel.BrandExp");
                    model.Profile.KeyAccount = (string)tool.GetValue(item, "ProfileLabel.KeyAccountExp");
                    model.Profile.Others = (string)tool.GetValue(item, "ProfileLabel.Others");

                    foreach (var sub in item["Experiences"].AsBsonArray) {
                        var expModel = new TalentExperience();
                        expModel.Company = tool.FindOne<Company>(tool.GetValue<string>(sub, "Company._id"));
                        expModel.Current = (bool?)tool.GetValue(sub, "CurrentJob");
                        expModel.FromYear = (int?)tool.GetValue(sub, "FromYear");
                        expModel.FromMonth = (int?)tool.GetValue(sub, "FromMonth");
                        expModel.ToYear = (int?)tool.GetValue(sub, "ToYear");
                        expModel.ToMonth = (int?)tool.GetValue(sub, "ToMonth");

                        expModel.Title = (string)tool.GetValue(sub, "Title");
                        expModel.Responsibility = (string)tool.GetValue(sub, "Responsibility");
                        expModel.Grade = (string)tool.GetValue(sub, "Grade");

                        expModel.AnnualPackage = (string)tool.GetValue(sub, "AnnualPackage");
                        expModel.Description = (string)tool.GetValue(sub, "Description");

                        expModel.BasicSalary = (int?)tool.GetValue(sub, "BasicSalary");
                        expModel.BasicSalaryMonths = (int?)tool.GetValue(sub, "BasicSalaryMonths");

                        expModel.Bonus = (string)tool.GetValue(sub, "Bonus");
                        expModel.Allowance = (string)tool.GetValue(sub, "Allowance");

                        model.Experience.Add(expModel);
                    }

                    tool.ImportAttachments(item, "Attachments", model.Attachment);
                }
            }
        }
    }
}