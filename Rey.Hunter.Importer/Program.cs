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

            ImportIndustry(db, mgr);
            ImportFunction(db, mgr);
            ImportLocation(db, mgr);
            ImportCategory(db, mgr);
            ImportChannel(db, mgr);
            ImportCompany(db, mgr);
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
            var company = db.GetCollection<BsonDocument>(NAME_COMPANY);
            var industry = db.GetCollection<BsonDocument>(NAME_INDUSTRY);

            var output = mgr.Company(ACCOUNT_ID);

            output.Drop();
            company.Find(x => true).ToList().ForEach(item => {
                var model = new Company();

                model.Name = item["Name"].AsString;
                model.Type = (CompanyType?)item["Type"].AsInt32;
                model.Status = (CompanyStatus?)item["Status"].AsInt32;
                model.Introduction = BsonTypeMapper.MapToDotNetValue(item["Introduction"]) as string;
                model.Culture = BsonTypeMapper.MapToDotNetValue(item["Culture"]) as string;
                model.BasicRecruitmentPrinciple = BsonTypeMapper.MapToDotNetValue(item["BasicRecruitmentPrinciple"]) as string;
                model.Industry.AddRange(item["Industries"].AsBsonArray.Select(x => FindNodeName(NAME_INDUSTRY, x["_id"].AsString)));

                output.InsertOne(model);
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
    }
}