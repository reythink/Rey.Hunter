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

        static void Main(string[] args) {
            var client = new MongoClient(new MongoClientSettings() {
                //Server = new MongoServerAddress("targetcareer.cn"),
                Credentials = new List<MongoCredential> { MongoCredential.CreateCredential("admin", "admin", "admin123~") },
            }.Freeze());
            var db = client.GetDatabase(NAME_DB);

            var mgr = new RepositoryManager();

            foreach (var account in ImportAccount(db, mgr)) {
                ImportRole(db, mgr, account);
                ImportUser(db, mgr, account);

                ImportIndustry(db, mgr, account);
                ImportFunction(db, mgr, account);
                ImportLocation(db, mgr, account);
                ImportCategory(db, mgr, account);
                ImportChannel(db, mgr, account);

                ImportCompany(db, mgr, account);
                ImportTalent(db, mgr, account);
                ImportProject(db, mgr, account);
            }
        }

        static IEnumerable<Account> ImportAccount(IMongoDatabase db, IRepositoryManager mgr) {
            var collection = db.GetCollection<BsonDocument>(NAME_ACCOUNT);
            var results = new List<Account>();

            collection.Find(x => true).ToList().ForEach(item => {
                var model = new Account();

                model.Id = (string)GetValue(item, "_id");
                model.CreateAt = (DateTime?)GetValue(item, "CreateAt");
                model.Company = (string)GetValue(item, "Company");
                model.Enabled = (bool)GetValue(item, "Enabled");

                results.Add(model);
            });

            var repAccount = mgr.Account();
            repAccount.Drop();
            repAccount.InsertMany(results);
            Console.WriteLine($"account: {results.Count}");

            return results;
        }

        static void ImportRole(IMongoDatabase db, IRepositoryManager mgr, Account account) {
            var filter = Builders<BsonDocument>.Filter.Eq("Account._id", account.Id);
            var collection = db.GetCollection<BsonDocument>(NAME_ROLE);
            var results = new List<Role>();

            collection.Find(filter).ToList().ForEach(item => {
                var model = new Role();

                model.Id = (string)GetValue(item, "_id");
                model.CreateAt = (DateTime?)GetValue(item, "CreateAt");
                model.Name = (string)GetValue(item, "Name");
                model.Enabled = (bool)GetValue(item, "Enabled");

                results.Add(model);
            });

            var repRole = mgr.Role(account);
            repRole.Drop();
            repRole.InsertMany(results);
            Console.WriteLine($"role: {results.Count}");
        }

        static void ImportUser(IMongoDatabase db, IRepositoryManager mgr, Account account) {
            var filter = Builders<BsonDocument>.Filter.Eq("Account._id", account.Id);
            var collection = db.GetCollection<BsonDocument>(NAME_USER);
            var repRole = mgr.Role(account);
            var results = new List<User>();

            collection.Find(filter).ToList().ForEach(item => {
                var model = new User();

                model.Id = (string)GetValue(item, "_id");
                model.CreateAt = (DateTime?)GetValue(item, "CreateAt");
                model.Email = (string)GetValue(item, "Email");
                model.Salt = (string)GetValue(item, "Salt");
                model.Password = (string)GetValue(item, "Password");
                model.Name = (string)GetValue(item, "Name");
                model.Enabled = (bool)GetValue(item, "Enabled");
                model.PortraitUrl = (string)GetValue(item, "PortraitUrl");
                model.Position = (string)GetValue(item, "Position");
                model.Role.AddRange(item["Roles"].AsBsonArray.Select(x => (RoleRef)repRole.FindOne(x["_id"].AsString)));

                results.Add(model);
            });

            var repUser = mgr.User(account);
            repUser.Drop();
            repUser.InsertMany(results);
            Console.WriteLine($"user: {results.Count}");
        }

        static IEnumerable<TModel> ImportNode<TModel, TModelRef>(IMongoCollection<BsonDocument> collection, IRepository<TModel> rep, Func<string, string, DateTime?, TModel> create, Account account)
            where TModel : AccountNodeModel<TModel, TModelRef>
            where TModelRef : class, INodeModelRef {
            var filter = Builders<BsonDocument>.Filter.Eq("Account._id", account.Id);
            var results = new List<TModel>();

            collection.Find(filter).ToList().ForEach(item => {
                var stack = new Stack<Tuple<BsonValue, TModel>>();
                item["Children"].AsBsonArray.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, TModel>(x, null)));

                while (stack.Count > 0) {
                    var node = stack.Pop();
                    var id = (string)GetValue(node.Item1.AsBsonDocument, "_id");
                    var name = (string)GetValue(node.Item1.AsBsonDocument, "Name");
                    var createAt = (DateTime?)GetValue(node.Item1.AsBsonDocument, "CreateAt");
                    var root = node.Item2 == null;
                    var model = create(id, name, createAt);
                    results.Add(model);

                    if (node.Item2 != null) {
                        model.SetParent(node.Item2);
                        //model.Parent = node.Item2;
                    }

                    var children = node.Item1["Children"].AsBsonArray;
                    if (children.Count > 0) {
                        children.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, TModel>(x, model)));
                    }
                }
            });

            rep.Drop();
            rep.InsertMany(results);
            return results;
        }

        static void ImportIndustry(IMongoDatabase db, IRepositoryManager mgr, Account account) {
            var collection = db.GetCollection<BsonDocument>(NAME_INDUSTRY);
            var results = ImportNode<Industry, IndustryRef>(collection, mgr.Industry(account), (id, name, createAt) => new Industry { Id = id, CreateAt = createAt, Name = name }, account);
            Console.WriteLine($"industry: {results.Count()}");
        }

        static void ImportFunction(IMongoDatabase db, IRepositoryManager mgr, Account account) {
            var collection = db.GetCollection<BsonDocument>(NAME_FUNCTION);
            var results = ImportNode<Function, FunctionRef>(collection, mgr.Function(account), (id, name, createAt) => new Function { Id = id, CreateAt = createAt, Name = name }, account);
            Console.WriteLine($"function: {results.Count()}");
        }

        static void ImportLocation(IMongoDatabase db, IRepositoryManager mgr, Account account) {
            var collection = db.GetCollection<BsonDocument>(NAME_LOCATION);
            var results = ImportNode<Location, LocationRef>(collection, mgr.Location(account), (id, name, createAt) => new Location { Id = id, CreateAt = createAt, Name = name }, account);
            Console.WriteLine($"location: {results.Count()}");
        }

        static void ImportCategory(IMongoDatabase db, IRepositoryManager mgr, Account account) {
            var collection = db.GetCollection<BsonDocument>(NAME_CATEGORY);
            var results = ImportNode<Category, CategoryRef>(collection, mgr.Category(account), (id, name, createAt) => new Category { Id = id, CreateAt = createAt, Name = name }, account);
            Console.WriteLine($"category: {results.Count()}");
        }

        static void ImportChannel(IMongoDatabase db, IRepositoryManager mgr, Account account) {
            var collection = db.GetCollection<BsonDocument>(NAME_CHANNEL);
            var results = ImportNode<Channel, ChannelRef>(collection, mgr.Channel(account), (id, name, createAt) => new Channel { Id = id, CreateAt = createAt, Name = name }, account);
            Console.WriteLine($"channel: {results.Count()}");
        }

        static void ImportCompany(IMongoDatabase db, IRepositoryManager mgr, Account account) {
            var filter = Builders<BsonDocument>.Filter.Eq("Account._id", account.Id);
            var collection = db.GetCollection<BsonDocument>(NAME_COMPANY);
            var repIndustry = mgr.Industry(account);
            var results = new List<Company>();

            collection.Find(filter).ToList().ForEach(item => {
                var model = new Company();

                model.Id = (string)GetValue(item, "_id");
                model.CreateAt = (DateTime?)GetValue(item, "CreateAt");
                model.Name = (string)GetValue(item, "Name");
                model.Type = (CompanyType?)(int)GetValue(item, "Type");
                model.Status = (CompanyStatus?)(int)GetValue(item, "Status");
                model.Introduction = (string)GetValue(item, "Introduction");
                model.Culture = (string)GetValue(item, "Culture");
                model.BasicRecruitmentPrinciple = (string)GetValue(item, "BasicRecruitmentPrinciple");
                model.Industry.AddRange(item["Industries"].AsBsonArray.Select(x => (IndustryRef)repIndustry.FindOne(x["_id"].AsString)));

                results.Add(model);
            });

            var repCompany = mgr.Company(account);
            repCompany.Drop();
            repCompany.InsertMany(results);
            Console.WriteLine($"company: {results.Count}");
        }

        static void ImportTalent(IMongoDatabase db, IRepositoryManager mgr, Account account) {
            var filter = Builders<BsonDocument>.Filter.Eq("Account._id", account.Id);
            var collection = db.GetCollection<BsonDocument>(NAME_TALENT);
            var repIndustry = mgr.Industry(account);
            var repFunction = mgr.Function(account);
            var repLocation = mgr.Location(account);
            var repCategory = mgr.Category(account);
            var repChannel = mgr.Channel(account);
            var repCompany = mgr.Company(account);

            var results = new List<Talent>();

            collection.Find(filter).ToList().ForEach(item => {
                var model = new Talent();

                model.Id = (string)GetValue(item, "_id");
                model.CreateAt = (DateTime?)GetValue(item, "CreateAt");
                model.Industry.AddRange(item["Industries"].AsBsonArray.Select(x => (IndustryRef)repIndustry.FindOne(x["_id"].AsString)));
                model.Function.AddRange(item["Functions"].AsBsonArray.Select(x => (FunctionRef)repFunction.FindOne(x["_id"].AsString)));
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

                model.Location.Current = item["CurrentLocations"].AsBsonArray.Select(x => (LocationRef)repLocation.FindOne(x["_id"].AsString)).FirstOrDefault();
                model.Location.Mobility.AddRange(item["MobilityLocations"].AsBsonArray.Select(x => (LocationRef)repLocation.FindOne(x["_id"].AsString)));

                model.Contact.Phone = (string)GetValue(item, "Phone");
                model.Contact.Mobile = (string)GetValue(item, "Mobile");
                model.Contact.Email = (string)GetValue(item, "Email");
                model.Contact.QQ = (string)GetValue(item, "QQ");
                model.Contact.Wechat = (string)GetValue(item, "Wechat");

                model.Profile.CrossIndustry.AddRange(item["ProfileLabel"]["CrossIndustries"].AsBsonArray.Select(x => (IndustryRef)repIndustry.FindOne(x["_id"].AsString)));
                model.Profile.CrossFunction.AddRange(item["ProfileLabel"]["CrossFunctions"].AsBsonArray.Select(x => (FunctionRef)repFunction.FindOne(x["_id"].AsString)));
                model.Profile.CrossChannel.AddRange(item["ProfileLabel"]["CrossChannels"].AsBsonArray.Select(x => (ChannelRef)repChannel.FindOne(x["_id"].AsString)));
                model.Profile.CrossCategory.AddRange(item["ProfileLabel"]["CrossCategories"].AsBsonArray.Select(x => (CategoryRef)repCategory.FindOne(x["_id"].AsString)));
                model.Profile.Brand = (string)GetValue(item["ProfileLabel"].AsBsonDocument, "BrandExp");
                model.Profile.KeyAccount = (string)GetValue(item["ProfileLabel"].AsBsonDocument, "KeyAccountExp");
                model.Profile.Others = (string)GetValue(item["ProfileLabel"].AsBsonDocument, "Others");

                foreach (var expItem in item["Experiences"].AsBsonArray) {
                    var companyId = (string)GetValue(expItem["Company"].AsBsonDocument, "_id");
                    var company = repCompany.FindOne(companyId);
                    if (company == null) {
                        Console.WriteLine($"Empty company, [id: {model.Id}][companyId: {companyId}]");
                        continue;
                    }

                    var expModel = new TalentExperience();
                    expModel.Company = company;
                    expModel.Current = (bool?)GetValue(expItem.AsBsonDocument, "CurrentJob");
                    expModel.FromYear = (int?)GetValue(expItem.AsBsonDocument, "FromYear");
                    expModel.FromMonth = (int?)GetValue(expItem.AsBsonDocument, "FromMonth");
                    expModel.ToYear = (int?)GetValue(expItem.AsBsonDocument, "ToYear");
                    expModel.ToMonth = (int?)GetValue(expItem.AsBsonDocument, "ToMonth");

                    expModel.Title = (string)GetValue(expItem.AsBsonDocument, "Title");
                    expModel.Responsibility = (string)GetValue(expItem.AsBsonDocument, "Responsibility");
                    expModel.Grade = (string)GetValue(expItem.AsBsonDocument, "Grade");

                    expModel.AnnualPackage = (string)GetValue(expItem.AsBsonDocument, "AnnualPackage");
                    expModel.Description = (string)GetValue(expItem.AsBsonDocument, "Description");

                    expModel.BasicSalary = (int?)GetValue(expItem.AsBsonDocument, "BasicSalary");
                    expModel.BasicSalaryMonths = (int?)GetValue(expItem.AsBsonDocument, "BasicSalaryMonths");

                    expModel.Bonus = (string)GetValue(expItem.AsBsonDocument, "Bonus");
                    expModel.Allowance = (string)GetValue(expItem.AsBsonDocument, "Allowance");

                    model.Experience.Add(expModel);
                }

                results.Add(model);
            });

            var repTalent = mgr.Talent(account);
            repTalent.Drop();
            repTalent.InsertMany(results);
            Console.WriteLine($"talent: {results.Count}");
        }

        static void ImportProject(IMongoDatabase db, IRepositoryManager mgr, Account account) {
            var filter = Builders<BsonDocument>.Filter.Eq("Account._id", account.Id);
            var collection = db.GetCollection<BsonDocument>(NAME_PROJECT);
            var repFunction = mgr.Function(account);
            var repLocation = mgr.Location(account);
            var repCompany = mgr.Company(account);
            var repUser = mgr.User(account);
            var repTalent = mgr.Talent(account);
            var results = new List<Project>();

            collection.Find(filter).ToList().ForEach(item => {
                var model = new Project();

                model.Id = (string)GetValue(item, "_id");
                model.CreateAt = (DateTime?)GetValue(item, "CreateAt");
                model.Position = (string)GetValue(item, "Name");
                model.Headcount = (int?)GetValue(item, "Headcount");
                model.Client = repCompany.FindOne((string)GetValue(item["Client"].AsBsonDocument, "_id"));

                model.Manager = repUser.FindOne((string)GetValue(item["Manager"].AsBsonDocument, "_id"));
                model.Consultant.AddRange(item["Consultants"].AsBsonArray.Select(x => (UserRef)repUser.FindOne(x["_id"].AsString)).Where(x => x != null));

                model.Function.AddRange(item["Functions"].AsBsonArray.Select(x => (FunctionRef)repFunction.FindOne(x["_id"].AsString)));
                //model.Location.AddRange(item["Locations"].AsBsonArray.Select(x => repLocation.FindOne(x["_id"].AsString).Name));

                model.AssignmentDate = (DateTime?)GetValue(item, "AssignmentDate");
                model.OfferSignedDate = (DateTime?)GetValue(item, "OfferSignedDate");
                model.OnBoardDate = (DateTime?)GetValue(item, "OnBoardDate");

                model.Notes = (string)GetValue(item, "Notes");

                foreach (var subItem in item["Candidates"].AsBsonArray) {
                    var talentId = (string)GetValue(subItem["Talent"].AsBsonDocument, "_id");
                    var talent = repTalent.FindOne(talentId);
                    if (talent == null) {
                        Console.WriteLine($"Empty talent, [id: {model.Id}][talent id: {talentId}]");
                        continue;
                    }

                    var subModel = new ProjectCandidate();
                    subModel.Talent = talent;
                    subModel.Status = (CandidateStatus)(int)GetValue(subItem.AsBsonDocument, "Status");

                    foreach (var subSubItem in subItem["Interviews"].AsBsonArray) {
                        subModel.Interviews.Add(new CandidateInterviewItem());
                    }

                    model.Candidate.Add(subModel);
                }

                model.Question.Question1 = (string)GetValue(item, "JobUnderstanding", "Field1");
                model.Question.Question2 = (string)GetValue(item, "JobUnderstanding", "Field2");
                model.Question.Question3 = (string)GetValue(item, "JobUnderstanding", "Field3");
                model.Question.Question4 = (string)GetValue(item, "JobUnderstanding", "Field4");
                model.Question.Question5 = (string)GetValue(item, "JobUnderstanding", "Field5");
                model.Question.Question6 = (string)GetValue(item, "JobUnderstanding", "Field6");
                model.Question.Question7 = (string)GetValue(item, "JobUnderstanding", "Field7");
                model.Question.Question8 = (string)GetValue(item, "JobUnderstanding", "Field8");
                model.Question.Question9 = (string)GetValue(item, "JobUnderstanding", "Field9");
                model.Question.Question10 = (string)GetValue(item, "JobUnderstanding", "Field10");
                model.Question.Question11 = (string)GetValue(item, "JobUnderstanding", "Field11");
                model.Question.Question12 = (string)GetValue(item, "JobUnderstanding", "Field12");

                results.Add(model);
            });

            var repProject = mgr.Project(account);
            repProject.Drop();
            repProject.InsertMany(results);
            Console.WriteLine($"project: {results.Count}");
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

        static object GetValue(BsonDocument item, params string[] names) {
            var value = item as BsonValue;

            foreach (var name in names) {
                if (!value.IsBsonDocument)
                    break;

                value = value.AsBsonDocument.GetValue(name, BsonNull.Value);
            }

            return BsonTypeMapper.MapToDotNetValue(value);
        }
    }
}