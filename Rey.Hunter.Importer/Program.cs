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

namespace Rey.Hunter.Importer {
    class Program {
        static void Main(string[] args) {
            var mgr = new RepositoryManager();
            var dbHunter = mgr.Client.GetDatabase("rey_hunter");
            var collCompany = dbHunter.GetCollection<BsonDocument>("bus.companies");
            var collIndustry = dbHunter.GetCollection<BsonDocument>("bas.industries");

            var repCompany = mgr.Company("58ff2e23a31baa1d28b77fd0");
            repCompany.Drop();

            var repIndustry = mgr.Industry("58ff2e23a31baa1d28b77fd0");

            //collCompany.Find(x => true).ToList().ForEach(item => {
            //    var model = new Company() {
            //        Name = item["Name"].AsString,
            //        Type = (CompanyType?)item["Type"].AsInt32,
            //        Status = (CompanyStatus?)item["Status"].AsInt32,
            //    };

            //    if (!item["Introduction"].IsBsonNull) {
            //        model.Introduction = item["Introduction"].AsString;
            //    }

            //    if (!item["SalaryStructure"].IsBsonNull) {
            //        model.SalaryStructure = item["SalaryStructure"].AsString;
            //    }

            //    if (!item["Culture"].IsBsonNull) {
            //        model.Culture = item["Culture"].AsString;
            //    }

            //    if (!item["BasicRecruitmentPrinciple"].IsBsonNull) {
            //        model.BasicRecruitmentPrinciple = item["BasicRecruitmentPrinciple"].AsString;
            //    }

            //    repCompany.InsertOne(model);
            //});

            repIndustry.Drop();
            collIndustry.Find(x => true).ToList().ForEach(item => {
                var content = item.ToJson();
                File.WriteAllText("D:\\industry.json", content);

                var stack = new Stack<BsonValue>();
                stack.Push(item);

                while (stack.Count > 0) {
                    var node = stack.Pop();

                    var children = node["Children"].AsBsonArray.ToList();
                    if (children.Count > 0) {
                        children.Reverse();
                        children.ForEach(child => stack.Push(child));
                    }
                }
            });
        }
    }
}