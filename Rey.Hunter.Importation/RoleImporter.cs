using Rey.Hunter.Models2;
using System;

namespace Rey.Hunter.Importation {
    public class RoleImporter : ImporterBase<Role>, IAccountImporter<Role> {
        public RoleImporter(IImportManager manager)
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
                }
            }
        }
    }
}