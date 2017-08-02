using MongoDB.Driver;
using Rey.Hunter.Models2;
using System.Collections.Generic;
using System;

namespace Rey.Hunter.Importation {
    public class ImportManager : IImportManager {
        public IMongoClient ImportClient { get; }
        public IMongoClient ExportClient { get; }

        public IImporter<Account> AccountImporter { get; }
        public List<IAccountImporter> Importers { get; } = new List<IAccountImporter>();

        public ImportManager() {
            var credential = MongoCredential.CreateCredential("admin", "admin", "admin123~");

            this.ImportClient = new MongoClient(new MongoClientSettings() {
                Credentials = new List<MongoCredential> { credential },
            }.Freeze());

            this.ExportClient = new MongoClient(new MongoClientSettings() {
                Credentials = new List<MongoCredential> { credential },
            }.Freeze());

            this.AccountImporter = new AccountImporter(this);
            this.Importers.Add(new RoleImporter(this));
            this.Importers.Add(new UserImporter(this));
            this.Importers.Add(new IndustryImporter(this));
            this.Importers.Add(new FunctionImporter(this));
            this.Importers.Add(new LocationImporter(this));
            this.Importers.Add(new CategoryImporter(this));
            this.Importers.Add(new ChannelImporter(this));
            this.Importers.Add(new CompanyImporter(this));
            this.Importers.Add(new TalentImporter(this));
            this.Importers.Add(new ProjectImporter(this));
            this.Importers.Add(new FileImporter(this));
        }

        public void Import() {
            foreach (var account in this.AccountImporter.Import()) {
                foreach (var importer in this.Importers) {
                    importer.Import(account);
                }
            }
        }
    }
}