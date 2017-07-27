using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rey.Hunter.Models2.Basic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rey.Hunter.Repository.Repositories {
    public class IndustryRepository : AccountModelRepositoryBase<Industry>, IIndustryRepository {
        public IndustryRepository(IRepositoryManager manager, string accountId)
            : base(manager, accountId) {
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
}
