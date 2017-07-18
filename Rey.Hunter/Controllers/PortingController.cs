using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OfficeOpenXml;
using Rey.Hunter.Models;
using Rey.Hunter.Models.Attributes;
using Rey.Hunter.Models.Basic;
using Rey.Hunter.Models.Business;
using Rey.Hunter.Models.Business.Enums;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class PortingController : ReyController {
        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        public IActionResult Import() {
            foreach (var formFile in this.Request.Form.Files) {
                if (formFile.Name.Equals("location", StringComparison.CurrentCultureIgnoreCase)) {
                    using (var input = formFile.OpenReadStream()) {
                        ImportLocation(input);
                    }
                } else if (formFile.Name.Equals("company", StringComparison.CurrentCultureIgnoreCase)) {
                    using (var input = formFile.OpenReadStream()) {
                        return ImportCompany(input);
                    }
                } else if (formFile.Name.Equals("talent", StringComparison.CurrentCultureIgnoreCase)) {
                    using (var input = formFile.OpenReadStream()) {
                        return ImportTalent(input);
                    }
                }
            }
            return Redirect("/Porting");
        }

        private void ImportLocation(Stream input) {
            var collection = this.GetMonCollection<LocationNode>();
            var package = new ExcelPackage(input);
            var sheet = package.Workbook.Worksheets.FirstOrDefault();
            if (sheet == null)
                throw new InvalidOperationException("There is no sheet in excel file!");

            var values = (object[,])sheet.Cells.Value;
            var rows = values.GetUpperBound(0) + 1;
            var columns = values.GetUpperBound(1) + 1;
            var account = this.CurrentAccount();
            var root = new LocationNode() { Account = account, Name = "root" };

            for (var row = 1; row <= rows; ++row) {
                var parent = root;

                for (var column = 1; column <= columns; ++column) {
                    var cell = sheet.Cells[row, column];
                    var value = cell.Value;
                    var name = value?.ToString();

                    if (name == null) {
                        parent = parent.Children.Last();
                        continue;
                    }

                    var node = new LocationNode() { Account = account, Name = name };
                    parent.Children.Add(node);
                }
            }

            var found = collection.FindOne(x => x.Account.Id.Equals(account.Id));
            if (found == null) {
                collection.InsertOne(root);
            } else {
                root.Id = found.Id;
                collection.ReplaceOne(x => x.Id.Equals(found.Id), root);
            }
        }

        private IActionResult ImportCompany(Stream input) {
            var errors = new ErrorManager();
            var account = this.CurrentAccount();
            var models = new List<ModelWrapper<Company>>();

            EachRow(input, (row, EachColumn) => {
                var model = new Company { Account = account, Source = DataSource.Excel };

                EachColumn((column, value) => {
                    switch (column) {
                        case 1: {
                                if (string.IsNullOrEmpty(value)) {
                                    errors.Error($"Empty name: [row: {row}][column: {column}]");
                                } else {
                                    model.Name = value;
                                }
                            }
                            break;
                        case 2: {
                                if (string.IsNullOrEmpty(value)) {
                                    errors.Error($"Empty industry: [row: {row}][column: {column}]");
                                } else {
                                    value.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(x => {
                                        var node = FindIndustry(x);
                                        if (node == null)
                                            errors.Error($"Cannot find industry: [row: {row}][column: {column}][value: {x}]");
                                        else
                                            model.Industries.Add(node);
                                    });
                                }
                            }
                            break;
                        case 3: {
                                if (!string.IsNullOrEmpty(value)) {
                                    value.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(x => {
                                        var node = FindIndustry(x);
                                        if (node == null)
                                            errors.Error($"Cannot find industry: [row: {row}][column: {column}][value: {x}]");
                                        else
                                            model.Industries.Add(node);
                                    });
                                }
                            }
                            break;
                        case 4: {
                                if (string.IsNullOrEmpty(value)) {
                                    errors.Error($"Empty type: [row: {row}][column: {column}]");
                                } else {
                                    model.Type = GetEnumValue<CompanyType>(value);
                                    if (model.Type == null)
                                        errors.Error($"Invalid company type: [row: {row}][column: {column}]");
                                }
                            }
                            break;
                        case 5: {
                                if (string.IsNullOrEmpty(value)) {
                                    errors.Error($"Empty status: [row: {row}][column: {column}]");
                                } else {
                                    model.Status = GetEnumValue<CompanyStatus>(value);
                                    if (model.Status == null)
                                        errors.Error($"Invalid company status: [row: {row}][column: {column}]");
                                }
                            }
                            break;
                        default:
                            break;
                    }
                });

                models.Add(new ModelWrapper<Company>(model, row));
            });

            models.GroupBy(x => x.Model.Name).ToList().ForEach(group => {
                if (group.Count() > 1) {
                    errors.Error($"Repeations items: [rows: { string.Join(", ", group.Select(x => x.Row)) }]");
                }
            });

            try {
                errors.ThrowIfError();

                var companies = this.GetMonCollection<Company>();
                var newModels = models.Select(x => x.Model).Where(model => !companies.Exist(x => x.Name.Equals(model.Name)));
                if (newModels.Count() > 0) {
                    companies.InsertMany(newModels);
                }

                return Content($"Great!!! there are no problems found. [total: {models.Count}][insert: {newModels.Count()}]");
            } catch (Exception ex) {
                return Content(ex.Message);
            }
        }

        private static List<Company> Companies { get; set; }
        private IActionResult ImportTalent(Stream input) {
            Companies = null;
            var errors = new ErrorManager();
            var account = this.CurrentAccount();
            var models = new List<ModelWrapper<Talent>>();

            EachRow(input, (row, EachColumn) => {
                var model = new Talent { Account = account, Source = DataSource.Excel, ProfileLabel = new TalentProfileLabel() };
                model.Experiences.Add(new TalentExperience() { CurrentJob = true });

                EachColumn((column, value) => {
                    switch (column) {
                        case 1: {
                                if (string.IsNullOrEmpty(value)) {
                                    errors.Error($"Empty function: [row: {row}][column: {column}]", column);
                                } else {
                                    value.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(x => {
                                        var node = FindFunction(x);
                                        if (node == null)
                                            errors.Error($"Cannot find function: [row: {row}][column: {column}][value: {x}]", column);
                                        else
                                            model.Functions.Add(node);
                                    });
                                }
                            }
                            break;
                        case 2: {
                                if (string.IsNullOrEmpty(value)) {
                                    errors.Error($"Empty industry: [row: {row}][column: {column}]", column);
                                } else {
                                    value.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(x => {
                                        var node = FindIndustry(x);
                                        if (node == null)
                                            errors.Error($"Cannot find industry: [row: {row}][column: {column}][value: {x}]", column);
                                        else
                                            model.Industries.Add(node);
                                    });
                                }
                            }
                            break;
                        case 3: {
                                if (string.IsNullOrEmpty(value)) {
                                    errors.Error($"Empty company: [row: {row}][column: {column}]", column);
                                } else {
                                    if (Companies == null) {
                                        Companies = this.GetMonCollection<Company>().MongoCollection
                                                    .Find(x => x.Account.Id.Equals(this.CurrentAccount().Id))
                                                    .ToList();
                                    }

                                    var company = Companies
                                                .Where(x => x.Name.Trim().Equals(value.Trim(), StringComparison.CurrentCultureIgnoreCase))
                                                .FirstOrDefault();

                                    if (company == null) {
                                        errors.Error($"Cannot find company: [row: {row}][column: {column}][value: {value}]", column);
                                    } else {
                                        model.Experiences.First().Company = company;
                                    }
                                }
                            }
                            break;
                        case 4: {
                                if (string.IsNullOrEmpty(value)) {
                                    errors.Error($"Empty title: [row: {row}][column: {column}]", column);
                                } else {
                                    model.Experiences.First().Title = value;
                                }
                            }
                            break;
                        case 5: {
                                model.Experiences.First().Responsibility = value?.Trim();
                            }
                            break;
                        case 6: {
                            }
                            break;
                        case 7: {
                                model.EnglishName = value?.Trim();
                            }
                            break;
                        case 8: {
                                model.ChineseName = value?.Trim();
                            }
                            break;
                        case 9: {
                                if (!string.IsNullOrEmpty(value)) {
                                    var year = 0;
                                    if (!int.TryParse(value.Trim(), out year)) {
                                        errors.Error($"Invalid DOB: [row: {row}][column: {column}][value: {value}]", column);
                                    } else {
                                        model.BirthYear = year;
                                    }
                                }
                            }
                            break;
                        case 10: {
                                if (!string.IsNullOrEmpty(value)) {
                                    if (value.Trim().Equals("F", StringComparison.CurrentCultureIgnoreCase)
                                     || value.Trim().Equals("Female", StringComparison.CurrentCultureIgnoreCase)) {
                                        model.Gender = Gender.Female;
                                    } else if (value.Trim().Equals("M", StringComparison.CurrentCultureIgnoreCase)
                                     || value.Trim().Equals("Male", StringComparison.CurrentCultureIgnoreCase)) {
                                        model.Gender = Gender.Male;
                                    } else {
                                        errors.Error($"Invalid Gender: [row: {row}][column: {column}][value: {value}]", column);
                                    }
                                }
                            }
                            break;
                        case 11: {
                                if (!string.IsNullOrEmpty(value)) {
                                    value.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(x => {
                                        var node = FindLocation(x.Trim());
                                        if (node == null)
                                            errors.Error($"Cannot find location: [row: {row}][column: {column}][value: {x}]", column);
                                        else
                                            model.CurrentLocations.Add(node);
                                    });
                                }
                            }
                            break;
                        case 12: {
                                if (!string.IsNullOrEmpty(value)) {
                                    value.Split(new char[] { '|', ',', '&', ';' }).Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(x => {
                                        var node = FindLocation(x.Trim());
                                        if (node == null)
                                            errors.Error($"Cannot find mobility: [row: {row}][column: {column}][value: {x}]", column);
                                        else
                                            model.MobilityLocations.Add(node);
                                    });
                                }
                            }
                            break;
                        case 13: {
                                if (!string.IsNullOrEmpty(value)) {
                                    var num = 0;
                                    if (!int.TryParse(value.Trim().Replace(",", ""), out num)) {
                                        errors.Error($"Annual total package numer parse failed: [row: {row}][column: {column}][value: {value}]", column);
                                    } else {
                                        model.Experiences.First().AnnualPackage = GetAnnualPackage(num);
                                    }
                                }
                            }
                            break;
                        case 14: {
                                model.Mobile = value?.Trim();
                            }
                            break;
                        case 15: {
                                model.Phone = value?.Trim();
                            }
                            break;
                        case 16: {
                                model.Email = value?.Trim();
                            }
                            break;
                        case 17: {
                                model.Wechat = value?.Trim();
                            }
                            break;
                        case 18: {
                                model.QQ = value?.Trim();
                            }
                            break;
                        case 19: {
                                if (!string.IsNullOrEmpty(value)) {
                                    value.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(x => {
                                        var node = FindFunction(x);
                                        if (node == null)
                                            errors.Error($"Cannot find function: [row: {row}][column: {column}][value: {x}]", column);
                                        else
                                            model.ProfileLabel.CrossFunctions.Add(node);
                                    });
                                }
                            }
                            break;
                        case 20: {
                                if (!string.IsNullOrEmpty(value)) {
                                    value.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(x => {
                                        var node = FindCategory(x);
                                        if (node == null)
                                            errors.Error($"Cannot find category: [row: {row}][column: {column}][value: {x}]", column);
                                        else
                                            model.ProfileLabel.CrossCategories.Add(node);
                                    });
                                }
                            }
                            break;
                        case 21: {
                                model.ProfileLabel.BrandExp = value?.Trim();
                            }
                            break;
                        case 22: {
                                if (!string.IsNullOrEmpty(value)) {
                                    value.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(x => {
                                        var node = FindChannel(x);
                                        if (node == null)
                                            errors.Error($"Cannot find channel: [row: {row}][column: {column}][value: {x}]", column);
                                        else
                                            model.ProfileLabel.CrossChannels.Add(node);
                                    });
                                }
                            }
                            break;
                        case 23: {
                                model.ProfileLabel.KeyAccountExp = value?.Trim();
                            }
                            break;
                        case 24: {
                                model.Linkedin = value?.Trim();
                            }
                            break;
                        case 25: {
                                model.Notes = value?.Trim();
                            }
                            break;
                        default:
                            break;
                    }
                });

                models.Add(new ModelWrapper<Talent>(model, row));
            });

            //models.GroupBy(x => x.Model.EnglishName).ToList().ForEach(group => {
            //    if (group.Count() > 1 && !string.IsNullOrEmpty(group.Key)) {
            //        errors.Error($"Repetitive items by english name \"{group.Key}\": [rows: { string.Join(", ", group.Select(x => x.Row)) }]", 101);
            //    }
            //});

            //models.GroupBy(x => x.Model.ChineseName).ToList().ForEach(group => {
            //    if (group.Count() > 1 && !string.IsNullOrEmpty(group.Key)) {
            //        errors.Error($"Repetitive items by chinese name \"{group.Key}\": [rows: { string.Join(", ", group.Select(x => x.Row)) }]", 102);
            //    }
            //});

            models.GroupBy(x => x.Model.Mobile).ToList().ForEach(group => {
                if (group.Count() > 1 && !string.IsNullOrEmpty(group.Key)) {
                    errors.Error($"Repetitive items by mobile phone \"{group.Key}\": [rows: { string.Join(", ", group.Select(x => x.Row)) }]", 103);
                }
            });

            try {
                errors.ThrowIfError();

                return Content($"Great!!! there are no problems found. [total: {models.Count}][insert: {0}]");
            } catch (Exception ex) {
                return Content(ex.Message);
            }
        }

        private void EachRow(Stream input, Action<int, Action<Action<int, string>>> eachRow) {
            var package = new ExcelPackage(input);
            var sheet = package.Workbook.Worksheets.FirstOrDefault();
            if (sheet == null)
                throw new InvalidOperationException("There is no sheet in excel file!");

            var values = (object[,])sheet.Cells.Value;
            var rows = values.GetUpperBound(0) + 1;
            var columns = values.GetUpperBound(1) + 1;

            for (var row = 2; row <= rows; ++row) {
                eachRow(row, (Action<int, string> eachColumn) => {
                    for (var column = 1; column <= columns; ++column) {
                        var value = sheet.Cells[row, column].Value?.ToString();
                        eachColumn(column, value);
                    }
                });
            }
        }

        private void EachItem(Stream input, Action<int, int, string> each) {
            var package = new ExcelPackage(input);
            var sheet = package.Workbook.Worksheets.FirstOrDefault();
            if (sheet == null)
                throw new InvalidOperationException("There is no sheet in excel file!");

            var values = (object[,])sheet.Cells.Value;
            var rows = values.GetUpperBound(0) + 1;
            var columns = values.GetUpperBound(1) + 1;

            for (var row = 2; row <= rows; ++row) {
                for (var column = 1; column <= columns; ++column) {
                    var value = sheet.Cells[row, column].Value?.ToString();
                    each(row, column, value);
                }
            }
        }

        private TNode FindNode<TNode>(TNode root, Func<TNode, bool> condition) where TNode : AccountNodeModel<TNode> {
            if (root == null)
                return null;

            if (condition.Invoke(root))
                return root;

            var stack = new Stack<TNode>();
            stack.Push(root);
            while (stack.Count > 0) {
                var node = stack.Pop();
                if (condition.Invoke(node))
                    return node;

                foreach (var child in node.Children) {
                    stack.Push(child);
                }
            }
            return null;
        }

        private TNode FindRootNode<TNode>() where TNode : AccountNodeModel<TNode> {
            return this.GetMonCollection<TNode>().FindOne(x => x.Account.Id.Equals(this.CurrentAccount().Id));
        }

        private static FunctionNode FunctionRootNode { get; set; }
        private FunctionNode FindFunction(string name) {
            if (FunctionRootNode == null) {
                FunctionRootNode = this.FindRootNode<FunctionNode>();
            }
            return FindNode(FunctionRootNode, node => node.Name.Trim().Equals(name.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        private static IndustryNode IndustryRootNode { get; set; }
        private IndustryNode FindIndustry(string name) {
            if (IndustryRootNode == null) {
                IndustryRootNode = this.FindRootNode<IndustryNode>();
            }
            return FindNode(IndustryRootNode, node => node.Name.Trim().Equals(name.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        private static LocationNode LocationRootNode { get; set; }
        private LocationNode FindLocation(string name) {
            if (LocationRootNode == null) {
                LocationRootNode = this.FindRootNode<LocationNode>();
            }
            return FindNode(LocationRootNode, node => node.Name.Trim().Equals(name.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        private static CategoryNode CategoryRootNode { get; set; }
        private CategoryNode FindCategory(string name) {
            if (CategoryRootNode == null) {
                CategoryRootNode = this.FindRootNode<CategoryNode>();
            }
            return FindNode(CategoryRootNode, node => node.Name.Trim().Equals(name.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        private static ChannelNode ChannelRootNode { get; set; }
        private ChannelNode FindChannel(string name) {
            if (ChannelRootNode == null) {
                ChannelRootNode = this.FindRootNode<ChannelNode>();
            }
            return FindNode(ChannelRootNode, node => node.Name.Trim().Equals(name.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        private object GetEnumValue(Type enumType, string value) {
            var fields = enumType.GetTypeInfo().GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields) {
                if (field.Name.Equals(value))
                    return Enum.Parse(enumType, field.Name);

                var attrs = field.GetCustomAttributes<DescriptionAttribute>();
                if (attrs.Any(x => x.Description.Equals(value))) {
                    return Enum.Parse(enumType, field.Name);
                }
            }
            return null;
        }

        private T? GetEnumValue<T>(string value) where T : struct {
            var ret = GetEnumValue(typeof(T), value);
            if (ret == null)
                return null;
            return (T)ret;
        }

        private string GetAnnualPackage(int value) {
            if (value < 0) return null;
            if (value <= 100000) return "0-100000";
            if (value <= 200000) return "100001-200000";
            if (value <= 300000) return "200001-300000";
            if (value <= 400000) return "300001-400000";
            if (value <= 500000) return "400001-500000";
            if (value <= 600000) return "500001-600000";
            if (value <= 700000) return "600001-700000";
            if (value <= 800000) return "700001-800000";
            if (value <= 900000) return "800001-900000";
            if (value <= 1000000) return "900001-1000000";
            if (value <= 1100000) return "1000001-1100000";
            if (value <= 1200000) return "1100001-1200000";
            if (value <= 1300000) return "1200001-1300000";
            if (value <= 1400000) return "1300001-1400000";
            if (value <= 1500000) return "1400001-1500000";
            if (value <= 1600000) return "1500001-1600000";
            if (value <= 1700000) return "1600001-1700000";
            if (value <= 1800000) return "1700001-1800000";
            if (value <= 1900000) return "1800001-1900000";
            if (value <= 2000000) return "1900001-2000000";
            if (value <= 2500000) return "2000000-2500000";
            if (value <= 3000000) return "2500001-3000000";
            return "3000001-";
        }
    }

    public class ErrorManager {
        private StringBuilder Content { get; } = new StringBuilder();
        private List<ErrorItem> Items { get; } = new List<ErrorItem>();

        public ErrorManager Error(string msg, int order = 0) {
            if (msg == null)
                throw new ArgumentNullException(nameof(msg));

            this.Items.Add(new ErrorItem(msg, order));
            return this;
        }

        public void ThrowIfError() {
            if (this.Items.Count == 0)
                return;

            var items = this.Items.OrderBy(x => x.Order).ToList();
            var builder = new StringBuilder();
            for (var i = 0; i < items.Count; ++i) {
                var item = items[i];
                builder.AppendLine($"{i + 1}.{item.Message}");
            }

            throw new Exception(builder.ToString());
        }

        private class ErrorItem {
            public string Message { get; }
            public int Order { get; }

            public ErrorItem(string message, int order = 0) {
                this.Message = message;
                this.Order = order;
            }
        }
    }

    public class ModelWrapper<TModel> {
        public TModel Model { get; }
        public int Row { get; }
        public ModelWrapper(TModel model, int row) {
            this.Model = model;
            this.Row = row;
        }
    }
}
