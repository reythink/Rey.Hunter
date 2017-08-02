using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;
using Rey.Hunter.Models2.Enums;
using System;
using System.Linq;

namespace Rey.Hunter.Importation {
    public class CompanyImporter : ImporterBase<Company>, IAccountImporter<Company> {
        public CompanyImporter(IImportManager manager)
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
                    model.Name = tool.GetValue<string>(item, "Name");

                    model.Type = (CompanyType?)(int)tool.GetValue(item, "Type");
                    model.Status = (CompanyStatus?)(int)tool.GetValue(item, "Status");
                    model.HR = tool.FindOne<Talent>(tool.GetValue<string>(item, "HR._id"));
                    model.LineManager = tool.FindOne<Talent>(tool.GetValue<string>(item, "LineManager._id"));

                    item.GetValue("Contacts").AsBsonArray.ToList().ForEach(sub => model.Address.Add(new CompanyAddress {
                        Name = tool.GetValue<string>(sub, "Name"),
                        Mobile = tool.GetValue<string>(sub, "Phone"),
                        Address = tool.GetValue<string>(sub, "Address"),
                    }));

                    tool.ImportAttachments(item, "DepartmentStructures", model.DepartmentStructures);
                    tool.ImportAttachments(item, "NameList", model.NameList);

                    model.Introduction = (string)tool.GetValue(item, "Introduction");
                    model.Culture = (string)tool.GetValue(item, "Culture");
                    model.BasicRecruitmentPrinciple = (string)tool.GetValue(item, "BasicRecruitmentPrinciple");
                    model.Industry.AddRange(tool.FindMany<Industry>(tool.GetIdList(item, "Industries")).Select(x => (IndustryRef)x));
                }
            }
        }
    }
}