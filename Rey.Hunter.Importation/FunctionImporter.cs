﻿using MongoDB.Bson;
using Rey.Hunter.Models2;
using Rey.Hunter.Models2.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rey.Hunter.Importation {
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
}