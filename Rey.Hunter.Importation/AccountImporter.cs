using Rey.Hunter.Models2;
using System;
using System.Collections.Generic;

namespace Rey.Hunter.Importation {
    public class AccountImporter : ImporterBase<Account>, IImporter<Account> {
        public AccountImporter(IImportManager manager)
            : base(manager) {
        }

        public IEnumerable<Account> Import() {
            var models = new List<Account>();
            using (var tool = this.BeginImportModel(models)) {
                var items = tool.GetImportItems();
                foreach (var item in items) {
                    var model = tool.CreateModel();
                    model.Id = tool.GetValue<string>(item, "_id");
                    model.CreateAt = tool.GetValue<DateTime?>(item, "CreateAt");
                    model.Company = tool.GetValue<string>(item, "Company");
                    model.Enabled = tool.GetValue<bool>(item, "Enabled");
                }
            }
            return models;
        }
    }
}