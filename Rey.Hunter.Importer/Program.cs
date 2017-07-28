using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;
using Rey.Hunter.Models2.Enums;
using Rey.Hunter.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rey.Hunter.Importer {
    class Program {
        public const string NAME_DB = "rey_hunter";

        public const string NAME_INDUSTRY = "bas.industries";
        public const string NAME_FUNCTION = "bas.functions";
        public const string NAME_LOCATION = "bas.locations";
        public const string NAME_CATEGORY = "bas.categories";
        public const string NAME_CHANNEL = "bas.channels";

        public const string NAME_COMPANY = "bus.companies";
        public const string NAME_TALENT = "bus.talents";
        public const string NAME_PROJECT = "bus.projects";

        public const string NAME_ACCOUNT = "ide.accounts";
        public const string NAME_ROLE = "ide.roles";
        public const string NAME_USER = "ide.users";

        public const string ACCOUNT_ID = "58ff2e23a31baa1d28b77fd0";

        static void Main(string[] args) {
            var client = new MongoClient(new MongoClientSettings() {
                Credentials = new List<MongoCredential> { MongoCredential.CreateCredential("admin", "admin", "admin123~") }
            }.Freeze());
            var db = client.GetDatabase(NAME_DB);

            var mgr = new RepositoryManager();

            //ImportIndustry(db, mgr);
            //ImportFunction(db, mgr);
            //ImportLocation(db, mgr);
            //ImportCategory(db, mgr);
            //ImportChannel(db, mgr);
            //ImportCompany(db, mgr);
            ImportTalent(db, mgr);
        }

        static void ImportNode<TModel>(IMongoCollection<BsonDocument> collection, IRepository<TModel> rep, Func<string, string, bool, TModel> create)
            where TModel : class, IModel, INodeModel {
            rep.Drop();
            collection.Find(x => true).ToList().ForEach(item => {
                var stack = new Stack<Tuple<BsonValue, TModel>>();
                item["Children"].AsBsonArray.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, TModel>(x, null)));

                while (stack.Count > 0) {
                    var node = stack.Pop();
                    var model = create(node.Item1["_id"].AsString, node.Item1["Name"].AsString, node.Item2 == null);
                    rep.InsertOne(model);

                    if (node.Item2 != null) {
                        node.Item2.Children.Add(model.Id);
                        rep.ReplaceOne(node.Item2);
                    }

                    var children = node.Item1["Children"].AsBsonArray;
                    if (children.Count > 0) {
                        children.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, TModel>(x, model)));
                    }
                }
            });
        }

        static void ImportIndustry(IMongoDatabase db, IRepositoryManager mgr) {
            var collection = db.GetCollection<BsonDocument>(NAME_INDUSTRY);
            ImportNode(collection, mgr.Industry(ACCOUNT_ID), (id, name, root) => new Industry { Id = id, Name = name, Root = root });
        }

        static void ImportFunction(IMongoDatabase db, IRepositoryManager mgr) {
            var collection = db.GetCollection<BsonDocument>(NAME_FUNCTION);
            ImportNode(collection, mgr.Function(ACCOUNT_ID), (id, name, root) => new Function { Id = id, Name = name, Root = root });
        }

        static void ImportLocation(IMongoDatabase db, IRepositoryManager mgr) {
            var collection = db.GetCollection<BsonDocument>(NAME_LOCATION);
            ImportNode(collection, mgr.Location(ACCOUNT_ID), (id, name, root) => new Location { Id = id, Name = name, Root = root });
        }

        static void ImportCategory(IMongoDatabase db, IRepositoryManager mgr) {
            var collection = db.GetCollection<BsonDocument>(NAME_CATEGORY);
            ImportNode(collection, mgr.Category(ACCOUNT_ID), (id, name, root) => new Category { Id = id, Name = name, Root = root });
        }

        static void ImportChannel(IMongoDatabase db, IRepositoryManager mgr) {
            var collection = db.GetCollection<BsonDocument>(NAME_CHANNEL);
            ImportNode(collection, mgr.Channel(ACCOUNT_ID), (id, name, root) => new Channel { Id = id, Name = name, Root = root });
        }

        static void ImportCompany(IMongoDatabase db, IRepositoryManager mgr) {
            var collection = db.GetCollection<BsonDocument>(NAME_COMPANY);
            var industry = mgr.Industry(ACCOUNT_ID);
            var company = mgr.Company(ACCOUNT_ID);

            company.Drop();
            collection.Find(x => true).ToList().ForEach(item => {
                var model = new Company();

                model.Id = item["_id"].AsString;
                model.Name = item["Name"].AsString;
                model.Type = (CompanyType?)(int)BsonTypeMapper.MapToDotNetValue(item["Type"]);
                model.Status = (CompanyStatus?)(int)BsonTypeMapper.MapToDotNetValue(item["Status"]);
                model.Introduction = BsonTypeMapper.MapToDotNetValue(item["Introduction"]) as string;
                model.Culture = BsonTypeMapper.MapToDotNetValue(item["Culture"]) as string;
                model.BasicRecruitmentPrinciple = BsonTypeMapper.MapToDotNetValue(item["BasicRecruitmentPrinciple"]) as string;
                model.Industry.AddRange(item["Industries"].AsBsonArray.Select(x => industry.FindOne(x["_id"].AsString).Name));

                company.InsertOne(model);
            });
        }

        static void ImportTalent(IMongoDatabase db, IRepositoryManager mgr) {
            var collection = db.GetCollection<BsonDocument>(NAME_TALENT);
            var industry = mgr.Industry(ACCOUNT_ID);
            var function = mgr.Function(ACCOUNT_ID);
            var location = mgr.Location(ACCOUNT_ID);
            var category = mgr.Category(ACCOUNT_ID);
            var channel = mgr.Channel(ACCOUNT_ID);
            var talent = mgr.Talent(ACCOUNT_ID);

            talent.Drop();
            collection.Find(x => true).ToList().ForEach(item => {
                var model = new Talent();

                model.Id = (string)GetValue(item, "_id");
                model.Industry.AddRange(item["Industries"].AsBsonArray.Select(x => industry.FindOne(x["_id"].AsString).Name));
                model.Function.AddRange(item["Functions"].AsBsonArray.Select(x => function.FindOne(x["_id"].AsString).Name));
                model.EnglishName = (string)GetValue(item, "EnglishName");
                model.ChineseName = (string)GetValue(item, "ChineseName");
                model.BirthYear = (int?)GetValue(item, "BirthYear");
                model.Gender = (Gender?)(int?)GetValue(item, "Gender");
                model.Marital = (Marital?)(int?)GetValue(item, "MaritalStatus");
                model.Education = (Education?)(int?)GetValue(item, "EducationLevel");
                model.Language = (Language?)(int?)GetValue(item, "Language");
                model.Nationality = (Nationality?)(int?)GetValue(item, "Nationality");
                model.Intension = (Intension?)(int?)GetValue(item, "Intension");
                model.Linkedin = (string)GetValue(item, "Linkedin");
                model.Vita = (string)GetValue(item, "CV");
                model.Notes = (string)GetValue(item, "Notes");

                model.Location.Current = item["CurrentLocations"].AsBsonArray.Select(x => location.FindOne(x["_id"].AsString).Name).FirstOrDefault();
                model.Location.Mobility.AddRange(item["MobilityLocations"].AsBsonArray.Select(x => location.FindOne(x["_id"].AsString).Name));

                model.Contact.Phone = (string)GetValue(item, "Phone");
                model.Contact.Mobile = (string)GetValue(item, "Mobile");
                model.Contact.Email = (string)GetValue(item, "Email");
                model.Contact.QQ = (string)GetValue(item, "QQ");
                model.Contact.Wechat = (string)GetValue(item, "Wechat");

                model.Profile.CrossIndustry.AddRange(item["ProfileLabel"]["CrossIndustries"].AsBsonArray.Select(x => industry.FindOne(x["_id"].AsString).Name));
                model.Profile.CrossFunction.AddRange(item["ProfileLabel"]["CrossFunctions"].AsBsonArray.Select(x => function.FindOne(x["_id"].AsString).Name));
                model.Profile.CrossChannel.AddRange(item["ProfileLabel"]["CrossChannels"].AsBsonArray.Select(x => channel.FindOne(x["_id"].AsString).Name));
                model.Profile.CrossCategory.AddRange(item["ProfileLabel"]["CrossCategories"].AsBsonArray.Select(x => category.FindOne(x["_id"].AsString).Name));
                model.Profile.Brand = (string)GetValue(item["ProfileLabel"].AsBsonDocument, "BrandExp");
                model.Profile.KeyAccount = (string)GetValue(item["ProfileLabel"].AsBsonDocument, "KeyAccountExp");
                model.Profile.Others = (string)GetValue(item["ProfileLabel"].AsBsonDocument, "Others");

                talent.InsertOne(model);
            });
        }

        static string FindNodeName(IMongoCollection<BsonDocument> collection, string id) {
            var stack = new Stack<BsonValue>();
            stack.Push(collection.Find(x => true).Single());

            while (stack.Count > 0) {
                var node = stack.Pop();
                if (node["_id"].AsString.Equals(id)) {
                    return node["Name"].AsString;
                }

                var children = node["Children"].AsBsonArray;
                if (children.Count > 0) {
                    children.Reverse().ToList().ForEach(x => stack.Push(x));
                }
            }
            return null;
        }

        static string FindNodeName(string collectionName, string id) {
            var collection = new RepositoryManager().Client.GetDatabase("rey_hunter").GetCollection<BsonDocument>(collectionName);
            return FindNodeName(collection, id);
        }

        static object GetValue(BsonDocument item, string name) {
            return BsonTypeMapper.MapToDotNetValue(item.GetValue(name, BsonNull.Value));
        }
    }
}