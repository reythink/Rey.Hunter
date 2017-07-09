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
            
            this.GetMonCollection<Company>().InsertMany(models.Select(x => x.Model));
        }

        private void ImportTalent(Stream input) {
            var errors = new ErrorManager();
            var collection = this.GetMonCollection<Talent>();
            var package = new ExcelPackage(input);
            var sheet = package.Workbook.Worksheets.FirstOrDefault();
            if (sheet == null)
                throw new InvalidOperationException("There is no sheet in excel file!");

            var values = (object[,])sheet.Cells.Value;
            var rows = values.GetUpperBound(0) + 1;
            var columns = values.GetUpperBound(1) + 1;
            var account = this.CurrentAccount();
            var items = new List<dynamic>();

            for (var row = 2; row <= rows; ++row) {
                var column1 = sheet.Cells[row, 1].Value?.ToString();        //! function
                var column2 = sheet.Cells[row, 2].Value?.ToString();        //! industry
                var column3 = sheet.Cells[row, 3].Value?.ToString();        //! company
                var column4 = sheet.Cells[row, 4].Value?.ToString();        //! title
                var column5 = sheet.Cells[row, 5].Value?.ToString();        //! in charge of

                dynamic item = new ExpandoObject();
                item.Column1 = column1;
                item.Column2 = column2;
                item.Column3 = column3;
                item.Column4 = column4;
                item.Column5 = column5;
                item.Row = row;

                var functions = new List<FunctionNode>();
                if (!string.IsNullOrEmpty(column1)) {
                    functions.AddRange(column1.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => {
                        var node = FindFunction(x);
                        if (node == null)
                            errors.Error($"Cannot find function! [row: {row}][column: 1][value: {x}]");
                        return node;
                    }));
                }

                var industries = new List<IndustryNode>();
                if (!string.IsNullOrEmpty(column2)) {
                    industries.AddRange(column2.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => {
                        var node = FindIndustry(x);
                        if (node == null)
                            errors.Error($"Cannot find industry! [row: {row}][column: 2][value: {x}]");
                        return node;
                    }));
                }

                if (string.IsNullOrEmpty(column3)) {
                    errors.Error($"Company is empty! [row: {row}]");
                } else {
                    var trimed = column3.Trim();
                    var company = this.GetMonCollection<Company>().FindOne(x => x.Name.Equals(column3) || x.Name.Equals(trimed));
                    if (company == null) {
                        errors.Error($"Cannot find company! [row: {row}][value: {column3}]");
                    }
                }

                var model = new Talent() {
                    Account = account,
                    Source = DataSource.Excel,
                };

                item.Model = model;
                items.Add(item);
            }

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
