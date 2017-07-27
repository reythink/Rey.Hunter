using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models2;
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

        public const string ACCOUNT_ID = "58ff2e23a31baa1d28b77fd0";

        static void Main(string[] args) {
            var client = new MongoClient(new MongoClientSettings() {
                Credentials = new List<MongoCredential> { MongoCredential.CreateCredential("admin", "admin", "admin123~") }
            }.Freeze());

            var mgr = new RepositoryManager();

            ImportIndustry(client, mgr);
            ImportCompany(client, mgr);
        }

        static void ImportIndustry(IMongoClient client, IRepositoryManager mgr) {
            var db = client.GetDatabase(NAME_DB);
            var industry = db.GetCollection<BsonDocument>(NAME_INDUSTRY);

            var output = mgr.Industry(ACCOUNT_ID);
            output.Drop();

            industry.Find(x => true).ToList().ForEach(item => {
                var stack = new Stack<Tuple<BsonValue, Industry>>();
                item["Children"].AsBsonArray.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, Industry>(x, null)));

                while (stack.Count > 0) {
                    var node = stack.Pop();
                    var model = new Industry {
                        Id = node.Item1["_id"].AsString,
                        Name = node.Item1["Name"].AsString,
                        Root = node.Item2 == null,
                    };
                    output.InsertOne(model);

                    if (node.Item2 != null) {
                        node.Item2.Children.Add(model);
                        output.ReplaceOne(node.Item2);
                    }

                    var children = node.Item1["Children"].AsBsonArray;
                    if (children.Count > 0) {
                        children.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, Industry>(x, model)));
                    }
                }
            });
        }

        static void ImportCompany(IMongoClient client, IRepositoryManager mgr) {
            var db = client.GetDatabase(NAME_DB);
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