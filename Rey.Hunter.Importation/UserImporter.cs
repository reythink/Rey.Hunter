using Rey.Hunter.Models2;
using System;
using System.Linq;

namespace Rey.Hunter.Importation {
    public class UserImporter : ImporterBase<User>, IAccountImporter<User> {
        public UserImporter(IImportManager manager)
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
                    model.Enabled = tool.GetValue<bool>(item, "Enabled");

                    model.Email = tool.GetValue<string>(item, "Email");
                    model.Salt = tool.GetValue<string>(item, "Salt");
                    model.Password = tool.GetValue<string>(item, "Password");

                    model.PortraitUrl = tool.GetValue<string>(item, "PortraitUrl");
                    model.Position = tool.GetValue<string>(item, "Position");
                    model.Role.AddRange(tool.FindMany<Role>(tool.GetIdList(item, "Roles")).Select(x => (RoleRef)x));
                }
            }
        }
    }
}