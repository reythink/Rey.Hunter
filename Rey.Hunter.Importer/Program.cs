using MongoDB.Bson;
using MongoDB.Driver;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Basic;
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
        static void Main(string[] args) {
            var mgr = new RepositoryManager();
            var dbHunter = mgr.Client.GetDatabase("rey_hunter");
            var collCompany = dbHunter.GetCollection<BsonDocument>("bus.companies");
            var collIndustry = dbHunter.GetCollection<BsonDocument>("bas.industries");
            var accountId = "58ff2e23a31baa1d28b77fd0";

            var repCompany = mgr.Company(accountId);
            repCompany.Drop();

            collCompany.Find(x => true).ToList().ForEach(item => {
                var model = new Company() {
                    Name = item["Name"].AsString,
                    Type = (CompanyType?)item["Type"].AsInt32,
                    Status = (CompanyStatus?)item["Status"].AsInt32,
                };

                SetProperty(model, x => x.Introduction, item["Introduction"]);
                SetProperty(model, x => x.Culture, item["Culture"]);
                SetProperty(model, x => x.BasicRecruitmentPrinciple, item["BasicRecruitmentPrinciple"]);
                SetIndustry(model.Industry, item["Industries"]);

                repCompany.InsertOne(model);
            });

            //repIndustry.Drop();
            //collIndustry.Find(x => true).ToList().ForEach(item => {
            //    var content = item.ToJson();
            //    File.WriteAllText("D:\\industry.json", content);

            //    var stack = new Stack<BsonValue>();
            //    stack.Push(item);l / 

            //    while (stack.Count > 0) {
            //        var node = stack.Pop();

            //        var children = node["Children"].AsBsonArray.ToList();
            //        if (children.Count > 0) {,                                                                              
            //            children.Reverse(); 
            //            children.ForEach(child => stackPush(child));
            //        }
            //    }                            
            //});

            //var industries = repIndustry.FindAll().ToList();
            //var children = industries.First().Children.Select(x => x.Concrete(mgr)).ToList();
        }

        static void SetProperty<TModel>(TModel model, Expression<Func<TModel, object>> exp, BsonValue value) {
            if (value.IsBsonNull)
                return;

            var member = exp.Body as MemberExpression;
            if (member == null)
                return;

            var prop = member.Member as PropertyInfo;
            if (prop == null)
                return;

            prop.SetValue(model, BsonTypeMapper.MapToDotNetValue(value));
        }

        static void SetIndustry(List<string> industry, BsonValue value) {
            industry.AddRange(value.AsBsonArray
                .Select(x => x["_id"].AsString)
                .Select(id => FindIndustry(id)));
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

        static string FindIndustry(string id) {
            return FindNodeName("bas.industries", id);
        }
    }
}