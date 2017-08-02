using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rey.Hunter.Importation {
    public class ImportTool<TModel> : IDisposable
        where TModel : class, IModel, new() {
        #region Constants

        public const string IMPORT_NAME_DB = "rey_hunter";

        public const string IMPORT_NAME_ACCOUNT = "ide.accounts";
        public const string IMPORT_NAME_ROLE = "ide.roles";
        public const string IMPORT_NAME_USER = "ide.users";

        public const string IMPORT_NAME_INDUSTRY = "bas.industries";
        public const string IMPORT_NAME_FUNCTION = "bas.functions";
        public const string IMPORT_NAME_LOCATION = "bas.locations";
        public const string IMPORT_NAME_CATEGORY = "bas.categories";
        public const string IMPORT_NAME_CHANNEL = "bas.channels";

        public const string IMPORT_NAME_COMPANY = "bus.companies";
        public const string IMPORT_NAME_TALENT = "bus.talents";
        public const string IMPORT_NAME_PROJECT = "bus.projects";


        public const string EXPORT_NAME_DB = "rey_test";

        public const string EXPORT_NAME_ACCOUNT = "auth.account";
        public const string EXPORT_NAME_ROLE = "auth.role";
        public const string EXPORT_NAME_USER = "auth.user";

        public const string EXPORT_NAME_INDUSTRY = "data.industry";
        public const string EXPORT_NAME_FUNCTION = "data.function";
        public const string EXPORT_NAME_LOCATION = "data.location";
        public const string EXPORT_NAME_CATEGORY = "data.category";
        public const string EXPORT_NAME_CHANNEL = "data.channel";

        public const string EXPORT_NAME_COMPANY = "busi.company";
        public const string EXPORT_NAME_TALENT = "busi.talent";
        public const string EXPORT_NAME_PROJECT = "busi.project";

        #endregion Constants

        protected IMongoClient ImportClient => this.Manager.ImportClient;
        protected IMongoDatabase ImportDB => this.ImportClient.GetDatabase(IMPORT_NAME_DB);

        protected IMongoClient ExportClient => this.Manager.ExportClient;
        protected IMongoDatabase ExportDB => this.ExportClient.GetDatabase(EXPORT_NAME_DB);

        private IImportManager Manager { get; }
        private List<TModel> Models { get; }
        public ImportTool(IImportManager manager, List<TModel> models) {
            this.Manager = manager;
            this.Models = models;
        }

        public ImportTool(IImportManager manager)
            : this(manager, new List<TModel>()) {
        }

        public static string GetImportCollectionName<T>() {
            var type = typeof(T);
            if (type.Equals(typeof(Account)))
                return IMPORT_NAME_ACCOUNT;

            if (type.Equals(typeof(Role)))
                return IMPORT_NAME_ROLE;

            if (type.Equals(typeof(User)))
                return IMPORT_NAME_USER;

            if (type.Equals(typeof(Industry)))
                return IMPORT_NAME_INDUSTRY;

            if (type.Equals(typeof(Function)))
                return IMPORT_NAME_FUNCTION;

            if (type.Equals(typeof(Location)))
                return IMPORT_NAME_LOCATION;

            if (type.Equals(typeof(Category)))
                return IMPORT_NAME_CATEGORY;

            if (type.Equals(typeof(Channel)))
                return IMPORT_NAME_CHANNEL;

            if (type.Equals(typeof(Company)))
                return IMPORT_NAME_COMPANY;

            if (type.Equals(typeof(Talent)))
                return IMPORT_NAME_TALENT;

            if (type.Equals(typeof(Project)))
                return IMPORT_NAME_PROJECT;

            return null;
        }

        public static string GetImportCollectionName() {
            return GetImportCollectionName<TModel>();
        }

        public static string GetExportCollectionName<T>() {
            var type = typeof(T);
            if (type.Equals(typeof(Account)))
                return EXPORT_NAME_ACCOUNT;

            if (type.Equals(typeof(Role)))
                return EXPORT_NAME_ROLE;

            if (type.Equals(typeof(User)))
                return EXPORT_NAME_USER;

            if (type.Equals(typeof(Industry)))
                return EXPORT_NAME_INDUSTRY;

            if (type.Equals(typeof(Function)))
                return EXPORT_NAME_FUNCTION;

            if (type.Equals(typeof(Location)))
                return EXPORT_NAME_LOCATION;

            if (type.Equals(typeof(Category)))
                return EXPORT_NAME_CATEGORY;

            if (type.Equals(typeof(Channel)))
                return EXPORT_NAME_CHANNEL;

            if (type.Equals(typeof(Company)))
                return EXPORT_NAME_COMPANY;

            if (type.Equals(typeof(Talent)))
                return EXPORT_NAME_TALENT;

            if (type.Equals(typeof(Project)))
                return EXPORT_NAME_PROJECT;

            return null;
        }

        public static string GetExportCollectionName() {
            return GetExportCollectionName<TModel>();
        }

        public IMongoCollection<BsonDocument> GetImportCollection(string name) {
            return this.ImportDB.GetCollection<BsonDocument>(name);
        }

        public IEnumerable<BsonDocument> GetImportItems(string name) {
            return this.GetImportCollection(name).Find(x => true).ToEnumerable();
        }

        public IEnumerable<BsonDocument> GetImportItems(string name, Account account) {
            return this.GetImportCollection(name).Find(Builders<BsonDocument>.Filter.Eq("Account._id", account.Id)).ToEnumerable();
        }

        public IEnumerable<BsonDocument> GetImportItems() {
            return this.GetImportItems(GetImportCollectionName());
        }

        public IEnumerable<BsonDocument> GetImportItems(Account account) {
            return this.GetImportItems(GetImportCollectionName(), account);
        }

        public TModel CreateModel() {
            var model = new TModel();
            this.Models.Add(model);
            return model;
        }

        public IEnumerable<T> ExportItems<T>(string name, IEnumerable<T> models) {
            this.ExportDB.DropCollection(name);
            this.ExportDB.GetCollection<T>(name).InsertMany(models);
            Console.WriteLine($"{typeof(T).Name}: {models.Count()}");
            return models;
        }

        public IEnumerable<T> ExportItems<T>(IEnumerable<T> models) {
            return this.ExportItems(GetExportCollectionName(), models);
        }

        public object GetValue(BsonValue value) {
            return BsonTypeMapper.MapToDotNetValue(value);
        }

        public T GetValue<T>(BsonValue value) {
            return (T)GetValue(value);
        }

        public object GetValue(BsonValue value, string name) {
            var names = name.Split('.');
            var retVal = value;
            foreach (var n in names) {
                retVal = retVal.AsBsonDocument[n];
            }
            return BsonTypeMapper.MapToDotNetValue(retVal);
        }

        public T GetValue<T>(BsonValue value, string name) {
            return (T)GetValue(value, name);
        }

        public IEnumerable<string> GetIdList(BsonValue value, string name) {
            var names = name.Split('.');
            var retVal = value;
            foreach (var n in names) {
                retVal = retVal.AsBsonDocument[n];
            }

            if (!retVal.IsBsonArray)
                throw new InvalidOperationException();

            return retVal.AsBsonArray.Select(x => this.GetValue<string>(x, "_id"));
        }

        public T FindOne<T>(string id)
            where T : class, IModel {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return this.ExportDB.GetCollection<T>(GetExportCollectionName<T>()).Find(filter).SingleOrDefault();
        }

        public TModel FindOne(string id) {
            return this.FindOne<TModel>(id);
        }

        public IEnumerable<T> FindMany<T>(IEnumerable<string> idList)
            where T : class, IModel {
            var filter = Builders<T>.Filter.In("_id", idList);
            return this.ExportDB.GetCollection<T>(GetExportCollectionName<T>()).Find(filter).ToEnumerable();
        }

        public IEnumerable<TModel> FindMany(IEnumerable<string> idList) {
            return this.FindMany<TModel>(idList);
        }

        public void Dispose() {
            this.ExportItems(this.Models);
        }
    }
}