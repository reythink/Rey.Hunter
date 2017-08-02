using MongoDB.Bson;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Business;
using Rey.Hunter.Models2.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rey.Hunter.Importation {
    class Program {
        static void Main(string[] args) {
            new ImportManager().Import();
        }
    }

    public class AccountImporter : ImporterBase<Account>, IImporter<Account> {
        public AccountImporter(IImportManager manager)
            : base(manager) {
        }

        public IEnumerable<Account> Import() {
            var models = new List<Account>();
            using (var tool = this.BeginImport(models)) {
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

    public class RoleImporter : ImporterBase<Role>, IAccountImporter<Role> {
        public RoleImporter(IImportManager manager)
            : base(manager) {
        }

        public void Import(Account account) {
            using (var tool = this.BeginImport()) {
                var items = tool.GetImportItems(account);
                foreach (var item in items) {
                    var model = tool.CreateModel();
                    model.Account = account;
                    model.Id = tool.GetValue<string>(item, "_id");
                    model.CreateAt = tool.GetValue<DateTime?>(item, "CreateAt");
                    model.Name = tool.GetValue<string>(item, "Name");
                    model.Enabled = tool.GetValue<bool>(item, "Enabled");
                }
            }
        }
    }

    public class UserImporter : ImporterBase<User>, IAccountImporter<User> {
        public UserImporter(IImportManager manager)
            : base(manager) {
        }

        public void Import(Account account) {
            using (var tool = this.BeginImport()) {
                var items = tool.GetImportItems(account);
                foreach (var item in items) {
                    var model = tool.CreateModel();
                    model.Account = account;
                    model.Id = tool.GetValue<string>(item, "_id");
                    model.CreateAt = tool.GetValue<DateTime?>(item, "CreateAt");
                    model.Name = tool.GetValue<string>(item, "Name");
                    model.Enabled = tool.GetValue<bool>(item, "Enabled");

                    model.Email = tool.GetValue<string>(item, "Email");
                    model.Salt = tool.GetValue<string>(item, "Salt");
                    model.Password = tool.GetValue<string>(item, "Password");

                    model.PortraitUrl = tool.GetValue<string>(item, "PortraitUrl");
                    model.Position = tool.GetValue<string>(item, "Position");
                    model.Role.AddRange(tool.FindMany<Role>(tool.GetIdList(item, "Roles")).Select(x => (RoleRef)x));
                }
            }
        }
    }

    public class IndustryImporter : ImporterBase<Industry>, IAccountImporter<Industry> {
        public IndustryImporter(IImportManager manager)
            : base(manager) {
        }

        public void Import(Account account) {
            using (var tool = this.BeginImport()) {
                var items = tool.GetImportItems(account);
                foreach (var item in items) {
                    var stack = new Stack<Tuple<BsonValue, Industry>>();
                    item["Children"].AsBsonArray.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, Industry>(x, null)));

                    while (stack.Count > 0) {
                        var node = stack.Pop();

                        var model = tool.CreateModel();
                        model.Account = account;
                        model.Id = tool.GetValue<string>(node.Item1, "_id");
                        model.Name = tool.GetValue<string>(node.Item1, "Name");
                        model.CreateAt = tool.GetValue<DateTime?>(node.Item1, "CreateAt");

                        if (node.Item2 != null) {
                            model.SetParent(node.Item2);
                        }

                        var children = node.Item1["Children"].AsBsonArray;
                        if (children.Count > 0) {
                            children.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, Industry>(x, model)));
                        }
                    }
                }
            }
        }
    }

    public class FunctionImporter : ImporterBase<Function>, IAccountImporter<Function> {
        public FunctionImporter(IImportManager manager)
            : base(manager) {
        }

        public void Import(Account account) {
            using (var tool = this.BeginImport()) {
                var items = tool.GetImportItems(account);
                foreach (var item in items) {
                    var stack = new Stack<Tuple<BsonValue, Function>>();
                    item["Children"].AsBsonArray.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, Function>(x, null)));

                    while (stack.Count > 0) {
                        var node = stack.Pop();

                        var model = tool.CreateModel();
                        model.Account = account;
                        model.Id = tool.GetValue<string>(node.Item1, "_id");
                        model.Name = tool.GetValue<string>(node.Item1, "Name");
                        model.CreateAt = tool.GetValue<DateTime?>(node.Item1, "CreateAt");

                        if (node.Item2 != null) {
                            model.SetParent(node.Item2);
                        }

                        var children = node.Item1["Children"].AsBsonArray;
                        if (children.Count > 0) {
                            children.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, Function>(x, model)));
                        }
                    }
                }
            }
        }
    }

    public class LocationImporter : ImporterBase<Location>, IAccountImporter<Location> {
        public LocationImporter(IImportManager manager)
            : base(manager) {
        }

        public void Import(Account account) {
            using (var tool = this.BeginImport()) {
                var items = tool.GetImportItems(account);
                foreach (var item in items) {
                    var stack = new Stack<Tuple<BsonValue, Location>>();
                    item["Children"].AsBsonArray.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, Location>(x, null)));

                    while (stack.Count > 0) {
                        var node = stack.Pop();

                        var model = tool.CreateModel();
                        model.Account = account;
                        model.Id = tool.GetValue<string>(node.Item1, "_id");
                        model.Name = tool.GetValue<string>(node.Item1, "Name");
                        model.CreateAt = tool.GetValue<DateTime?>(node.Item1, "CreateAt");

                        if (node.Item2 != null) {
                            model.SetParent(node.Item2);
                        }

                        var children = node.Item1["Children"].AsBsonArray;
                        if (children.Count > 0) {
                            children.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, Location>(x, model)));
                        }
                    }
                }
            }
        }
    }

    public class CategoryImporter : ImporterBase<Category>, IAccountImporter<Category> {
        public CategoryImporter(IImportManager manager)
            : base(manager) {
        }

        public void Import(Account account) {
            using (var tool = this.BeginImport()) {
                var items = tool.GetImportItems(account);
                foreach (var item in items) {
                    var stack = new Stack<Tuple<BsonValue, Category>>();
                    item["Children"].AsBsonArray.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, Category>(x, null)));

                    while (stack.Count > 0) {
                        var node = stack.Pop();

                        var model = tool.CreateModel();
                        model.Account = account;
                        model.Id = tool.GetValue<string>(node.Item1, "_id");
                        model.Name = tool.GetValue<string>(node.Item1, "Name");
                        model.CreateAt = tool.GetValue<DateTime?>(node.Item1, "CreateAt");

                        if (node.Item2 != null) {
                            model.SetParent(node.Item2);
                        }

                        var children = node.Item1["Children"].AsBsonArray;
                        if (children.Count > 0) {
                            children.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, Category>(x, model)));
                        }
                    }
                }
            }
        }
    }

    public class ChannelImporter : ImporterBase<Channel>, IAccountImporter<Channel> {
        public ChannelImporter(IImportManager manager)
            : base(manager) {
        }

        public void Import(Account account) {
            using (var tool = this.BeginImport()) {
                var items = tool.GetImportItems(account);
                foreach (var item in items) {
                    var stack = new Stack<Tuple<BsonValue, Channel>>();
                    item["Children"].AsBsonArray.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, Channel>(x, null)));

                    while (stack.Count > 0) {
                        var node = stack.Pop();

                        var model = tool.CreateModel();
                        model.Account = account;
                        model.Id = tool.GetValue<string>(node.Item1, "_id");
                        model.Name = tool.GetValue<string>(node.Item1, "Name");
                        model.CreateAt = tool.GetValue<DateTime?>(node.Item1, "CreateAt");

                        if (node.Item2 != null) {
                            model.SetParent(node.Item2);
                        }

                        var children = node.Item1["Children"].AsBsonArray;
                        if (children.Count > 0) {
                            children.Reverse().ToList().ForEach(x => stack.Push(new Tuple<BsonValue, Channel>(x, model)));
                        }
                    }
                }
            }
        }
    }

    public class CompanyImporter : ImporterBase, IAccountImporter<Company> {
        public CompanyImporter(IImportManager manager)
            : base(manager) {
        }

        public void Import(Account account) {
        }
    }

    public class TalentImporter : ImporterBase, IAccountImporter<Talent> {
        public TalentImporter(IImportManager manager)
            : base(manager) {
        }

        public void Import(Account account) {
        }
    }

    public class ProjectImporter : ImporterBase, IAccountImporter<Project> {
        public ProjectImporter(IImportManager manager)
            : base(manager) {
        }

        public void Import(Account account) {
        }
    }
}