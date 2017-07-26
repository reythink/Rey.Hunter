using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Rey.Hunter.Models2;
using System.Collections.Generic;
using System;
using Rey.Hunter.Models2.Basic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Rey.Hunter.Repository {
    public interface IRoleRepository : IAccountModelRepository<Role> {

    }

    public class RoleRepository : AccountModelRepositoryBase<Role>, IRoleRepository {
        public RoleRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }

        public override string GetCollectionName() {
            return "role";
        }
    }

    public interface IUserRepository : IAccountModelRepository<User> {

    }

    public class UserRepository : AccountModelRepositoryBase<User>, IUserRepository {
        public UserRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }

        public override string GetCollectionName() {
            return "user";
        }
    }

    public interface IIndustryRepository : IAccountModelRepository<Industry> {
        void Initialize();
    }

    public class IndustryRepository : AccountModelRepositoryBase<Industry>, IIndustryRepository {
        public IndustryRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
        }

        public override string GetCollectionName() {
            return "industry";
        }

        public void Initialize() {
            var dir = Directory.GetCurrentDirectory();
            var path = Path.Combine(dir, "Data", "industry.json");
            var content = File.ReadAllText(path);
            var items = JsonConvert.DeserializeObject(content) as JArray;
            var type = items.GetType();
            var stack = new Stack<Tuple<JToken, Industry>>();
            items.Reverse().ToList().ForEach(item => stack.Push(new Tuple<JToken, Industry>(item, null)));

            while (stack.Count > 0) {
                var node = stack.Pop();
                var model = new Industry { Name = node.Item1["name"].Value<string>() };
                this.InsertOne(model);

                if (node.Item2 != null) {
                    node.Item2.Children.Add(model);
                    this.ReplaceOne(node.Item2);
                }

                var children = node.Item1["children"] as JArray;
                if (children != null && children.Count > 0) {
                    children.Reverse().ToList().ForEach(child => stack.Push(new Tuple<JToken, Industry>(child, model)));
                }
            }
        }
    }


    public class RepositoryManager : IRepositoryManager {
        public IMongoClient Client { get; }

        public RepositoryManager() {
            ConventionRegistry.Register("CamelCase", new ConventionPack { new CamelCaseElementNameConvention() }, type => true);

            var settings = new MongoClientSettings() {
                Credentials = new List<MongoCredential> { MongoCredential.CreateCredential("admin", "admin", "admin123~") }
            };
            this.Client = new MongoClient(settings.Freeze());
        }

        public IAccountRepository Account() {
            return new AccountRepository(this);
        }

        public IRoleRepository Role(string accountId) {
            return new RoleRepository(this, accountId);
        }

        public IUserRepository User(string accountId) {
            return new UserRepository(this, accountId);
        }

        public IIndustryRepository Industry(string accountId) {
            return new IndustryRepository(this, accountId);
        }

        public ICompanyRepository Company(string accountId) {
            return new CompanyRepository(this, accountId);
        }
    }
}
