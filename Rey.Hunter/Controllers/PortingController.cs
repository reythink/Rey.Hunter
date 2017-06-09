using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Rey.Hunter.Models.Basic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}
