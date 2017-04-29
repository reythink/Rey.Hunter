using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Rey.Hunter {
    public class Build {
        public int Number { get; }
        public DateTime Date { get; }
        public List<Change> Changes { get; } = new List<Change>();
        public Build(int number, DateTime date) {
            this.Number = number;
            this.Date = date;
        }
    }

    public class Change {
        public string Comment { get; }
        public Change(string comment) {
            this.Comment = comment;
        }
    }
}

namespace Rey.Hunter.Controllers {
    [Authorize]
    public class ChangeLogController : ReyController {
        private HttpClient Client { get; } = new HttpClient();

        public async Task<IActionResult> Index() {
            var host = "http://targetcareer.cn:9080";
            var project = "ReyHunter2";
            var result = new List<Build>();

            var content = await this.GetStringAsync($"{host}/guestAuth/app/rest/builds?locator=project:(id:{project})");
            var buildItems = JsonConvert.DeserializeObject<dynamic>(content);

            foreach (var buildRef in buildItems.build) {
                if (result.Count > 20) { break; }

                content = await this.GetStringAsync($"{host}{buildRef.href}");
                var buildItem = JsonConvert.DeserializeObject<dynamic>(content);
                var build = new Build((int)buildItem.number, DateTime.ParseExact((string)buildItem.finishDate, "yyyyMMddTHHmmsszzz", null));
                result.Add(build);

                content = await this.GetStringAsync($"{host}{buildItem.changes.href}");
                var changeItems = JsonConvert.DeserializeObject<dynamic>(content);

                if (changeItems.change == null)
                    continue;

                foreach (var changeRef in changeItems.change) {
                    content = await this.GetStringAsync($"{host}{changeRef.href}");
                    var changeItem = JsonConvert.DeserializeObject<dynamic>(content);
                    var change = new Change((string)changeItem.comment);
                    build.Changes.Add(change);
                }
            }

            return View(result);
        }

        private async Task<string> GetStringAsync(string uri) {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            return await (await this.Client.SendAsync(request)).Content.ReadAsStringAsync();
        }
    }
}
