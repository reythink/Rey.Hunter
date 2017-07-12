using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                        ImportCompany(input);
                    }
                } else if (formFile.Name.Equals("talent", StringComparison.CurrentCultureIgnoreCase)) {
                    using (var input = formFile.OpenReadStream()) {
                        ImportTalent(input);
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

        private void ImportCompany(Stream input) {
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

            errors.Throw();

            var companies = this.GetMonCollection<Company>();
            var newModels = models.Select(x => x.Model).Where(model => !companies.Exist(x => x.Name.Equals(model.Name)));
            if (newModels.Count() > 0) {
                companies.InsertMany(newModels);
            }
        }

        private void ImportTalent(Stream input) {
            var errors = new ErrorManager();
            var account = this.CurrentAccount();
            var models = new List<ModelWrapper<Talent>>();

            EachRow(input, (row, EachColumn) => {
                var model = new Talent { Account = account, Source = DataSource.Excel };
                model.Experiences.Add(new TalentExperience() { CurrentJob = true });

                EachColumn((column, value) => {
                    switch (column) {
                        case 1: {
                                //if (string.IsNullOrEmpty(value)) {
                                //    errors.Error($"Empty function: [row: {row}][column: {column}]");
                                //} else {
                                //    value.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(x => {
                                //        var node = FindFunction(x);
                                //        if (node == null)
                                //            errors.Error($"Cannot find function: [row: {row}][column: {column}][value: {x}]");
                                //        else
                                //            model.Functions.Add(node);
                                //    });
                                //}
                            }
                            break;
                        case 2: {
                                //if (string.IsNullOrEmpty(value)) {
                                //    errors.Error($"Empty industry: [row: {row}][column: {column}]");
                                //} else {
                                //    value.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(x => {
                                //        var node = FindIndustry(x);
                                //        if (node == null)
                                //            errors.Error($"Cannot find industry: [row: {row}][column: {column}][value: {x}]");
                                //        else
                                //            model.Industries.Add(node);
                                //    });
                                //}
                            }
                            break;
                        case 3: {
                                //if (string.IsNullOrEmpty(value)) {
                                //    errors.Error($"Empty company: [row: {row}][column: {column}]");
                                //} else {
                                //    var company = this.GetMonCollection<Company>().FindOne(x => x.Name.Equals(value));
                                //    if (company == null) {
                                //        errors.Error($"Cannot find company: [row: {row}][column: {column}][value: {value}]");
                                //    } else {
                                //        model.Experiences.First().Company = company;
                                //    }
                                //}
                            }
                            break;
                        case 4: {
                                if (string.IsNullOrEmpty(value)) {
                                    errors.Error($"Empty title: [row: {row}][column: {column}]");
                                } else {
                                    model.Experiences.First().Title = value;
                                }
                            }
                            break;
                        case 5: {
                                model.Experiences.First().Responsibility = value;
                            }
                            break;
                        case 6: {
                            }
                            break;
                        case 7: {
                                model.EnglishName = value;
                            }
                            break;
                        case 8: {
                                model.ChineseName = value;
                            }
                            break;
                        case 9: {
                                if (!string.IsNullOrEmpty(value)) {
                                    var year = 0;
                                    if (!int.TryParse(value.Trim(), out year)) {
                                        errors.Error($"Invalid DOB: [row: {row}][column: {column}][value: {value}]");
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
                                        errors.Error($"Invalid Gender: [row: {row}][column: {column}][value: {value}]");
                                    }
                                }
                            }
                            break;
                        case 11: {
                                if (string.IsNullOrEmpty(value)) {
                                    errors.Error($"Empty location: [row: {row}][column: {column}]");
                                } else {
                                    value.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(x => {
                                        var node = FindLocation(x);
                                        if (node == null)
                                            errors.Error($"Cannot find location: [row: {row}][column: {column}][value: {x}]");
                                        else
                                            model.CurrentLocations.Add(node);
                                    });
                                }
                            }
                            break;
                        case 12: {
                                if (!string.IsNullOrEmpty(value)) {
                                    value.Split(new char[] { '|', ',', '&' }).Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ForEach(x => {
                                        var node = FindLocation(x);
                                        if (node == null)
                                            errors.Error($"Cannot find mobility: [row: {row}][column: {column}][value: {x}]");
                                        else
                                            model.MobilityLocations.Add(node);
                                    });
                                }
                            }
                            break;
                        default:
                            break;
                    }
                });

                models.Add(new ModelWrapper<Talent>(model, row));
            });

            errors.Throw();
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

        private TNode FindNode<TNode>(Func<TNode, bool> condition) where TNode : AccountNodeModel<TNode> {
            var root = this.GetMonCollection<TNode>().FindOne(x => x.Account.Id.Equals(this.CurrentAccount().Id));
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

        private FunctionNode FindFunction(string name) {
            return FindNode<FunctionNode>(node => node.Name.Trim().Equals(name.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        private IndustryNode FindIndustry(string name) {
            return FindNode<IndustryNode>(node => node.Name.Trim().Equals(name.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        private LocationNode FindLocation(string name) {
            return FindNode<LocationNode>(node => node.Name.Trim().Equals(name.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        public object GetEnumValue(Type enumType, string value) {
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

        public T? GetEnumValue<T>(string value) where T : struct {
            var ret = GetEnumValue(typeof(T), value);
            if (ret == null)
                return null;
            return (T)ret;
        }
    }

    public class ErrorManager {
        private StringBuilder Content { get; } = new StringBuilder();

        public ErrorManager Error(string msg) {
            if (msg == null)
                throw new ArgumentNullException(nameof(msg));

            Content.AppendLine(msg);
            return this;
        }

        public void Throw() {
            var message = this.Content.ToString();
            if (string.IsNullOrEmpty(message))
                return;

            throw new Exception(message);
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
