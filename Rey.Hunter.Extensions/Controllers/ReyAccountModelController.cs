using Newtonsoft.Json;
using Rey.Hunter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc {
    public class ReyAccountModelController<TModel> : ReyStringModelController<TModel>
        where TModel : AccountModel {
        public ReyAccountModelController() {
            this.BeforeQuery += query => query.Where(x => x.Account.Id.Equals(this.CurrentAccount().Id));

            this.BeforeCreate += model => {
                this.AttachCurrentAccount(model);
                this.AttachCurrentUser(model);
            };
        }

        [HttpGet("export")]
        public Task<IActionResult> Export() {
            return this.InvokeAsync(() => {
                var list = this.Collection.FindMany(x => x.Account.Id.Equals(this.CurrentAccount().Id));
                var contentType = "text/json";
                var fileName = string.Format("{0}_{1}.json",
                    typeof(TModel).Name.Replace("Node", ""),
                    DateTime.Now.ToString("yyyyMMddHHmmss"));
                var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(list));
                return File(buffer, contentType, fileName);
            });
        }

        [HttpPost("import")]
        public Task<IActionResult> Import() {
            return this.JsonInvokeAsync(() => {
                foreach (var formFile in this.Request.Form.Files) {
                    using (var input = formFile.OpenReadStream()) {
                        using (var reader = new System.IO.StreamReader(input)) {
                            var content = reader.ReadToEnd();
                            var list = JsonConvert.DeserializeObject<List<TModel>>(content);
                            foreach (var model in list) {
                                this.AttachCurrentAccount(model);
                                this.AttachCurrentUser(model);
                                this.Repository.Upsert(model);
                            }
                        }
                    }
                }
            });
        }
    }
}
