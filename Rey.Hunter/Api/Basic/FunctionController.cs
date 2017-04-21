using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Basic;
using System.Collections.Generic;

namespace Rey.Hunter.Api.Basic {
    [Route("/Api/[controller]/")]
    public class FunctionController : ReyTreeController<FunctionNode, string> {
        protected override FunctionNode CreateRootNode() {
            return new FunctionNode() { Name = "root", Account = this.CurrentAccount() };
        }

        protected override void DeleteRootNode() {
            this.Collection.DeleteOne(x => x.Account.Id == this.CurrentAccount().Id);
        }

        protected override FunctionNode FindRootNode() {
            return this.Collection.FindOne(x => x.Account.Id == this.CurrentAccount().Id);
        }

        protected override List<FunctionNode> GetChildNodes(FunctionNode node) {
            return node?.Children;
        }

        protected override FunctionNode GetUpdateNode(FunctionNode oldNode, FunctionNode newNode) {
            oldNode.Name = newNode.Name;
            return oldNode;
        }

        protected override void ReplaceRootNode(FunctionNode root) {
            this.Collection.UpdateOne(x => x.Account.Id == this.CurrentAccount().Id, builder => builder.Set(x => x.Name, root.Name).Set(x => x.Children, root.Children));
        }
    }
}
