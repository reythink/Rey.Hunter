using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
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
            var collection = this.GetMonCollection<Company>();
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
                var column1 = sheet.Cells[row, 1].Value?.ToString();        //! name
                var column2 = sheet.Cells[row, 2].Value?.ToString();        //! industry
                var column3 = sheet.Cells[row, 3].Value?.ToString();        //! industry
                var column4 = sheet.Cells[row, 4].Value?.ToString();        //! type
                var column5 = sheet.Cells[row, 5].Value?.ToString();        //! status

                dynamic item = new ExpandoObject();
                item.Column1 = column1;
                item.Column2 = column2;
                item.Column3 = column3;
                item.Column4 = column4;
                item.Column5 = column5;
                item.Row = row;

                var industries = new List<IndustryNode>();
                if (!string.IsNullOrEmpty(column2)) {
                    industries.AddRange(column2.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => FindIndustry(x)));
                }

                if (!string.IsNullOrEmpty(column3)) {
                    industries.AddRange(column3.Split('|').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => FindIndustry(x)));
                }

                foreach (var industry in industries) {
                    if (industry == null)
                        throw new InvalidOperationException($"Cannot find industry! [row: {row}][content1: {column2}][content2: {column3}]");
                }

                var model = new Company {
                    Account = account,
                    Source = DataSource.Excel,
                    Name = column1,
                };

                foreach (var industry in industries) {
                    model.Industries.Add(industry);
                }

                var type = GetEnumValue(typeof(CompanyType), column4);
                if (type == null)
                    throw new InvalidOperationException($"Type parse failed! [row: {row}][content: {column4}]");

                model.Type = (CompanyType)type;

                var status = GetEnumValue(typeof(CompanyStatus), column5);
                if (status == null)
                    throw new InvalidOperationException($"Status parse failed! [row: {row}][content: {column5}]");

                model.Status = (CompanyStatus)status;

                item.Model = model;
                items.Add(item);
            }

            var groups = items.GroupBy(x => (string)x.Column1);
            var exceptions = new List<string>();
            foreach (var group in groups) {
                if (group.Count() > 1) {
                    exceptions.Add($"[name: {group.Key}][rows: { string.Join(",", group.Select(x => x.Row)) }]");
                }
            }

            if (exceptions.Count > 0)
                throw new InvalidOperationException($"Repeation:\r\n{string.Join("\r\n", exceptions)}");

            //collection.DeleteMany(x => x.Source == DataSource.Excel);
            collection.InsertMany(items.Select(x => (Company)x.Model));
        }

        private IndustryNode FindIndustry(string name) {
            var root = this.GetMonCollection<IndustryNode>().FindOne(x => x.Account.Id.Equals(this.CurrentAccount().Id));
            if (root == null)
                return null;

            if (root.Name.Trim().Equals(name))
                return root;

            var stack = new Stack<IndustryNode>();
            stack.Push(root);
            while (stack.Count > 0) {
                var node = stack.Pop();
                if (node.Name.Trim().Equals(name.Trim(), StringComparison.CurrentCultureIgnoreCase))
                    return node;

                foreach (var child in node.Children) {
                    stack.Push(child);
                }
            }
            return null;
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
    }
}
