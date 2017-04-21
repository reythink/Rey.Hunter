using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Rey.Hunter.Models.Basic;
using System.Collections.Generic;

namespace Rey.Hunter.Api.Basic {
    [Route("/Api/[controller]/")]
    public class IndustryController : ReyTreeController<IndustryNode, string> {
        protected override IndustryNode CreateRootNode() {
            return new IndustryNode() { Name = "root", Account = this.CurrentAccount() };
        }

        protected override void DeleteRootNode() {
            this.Collection.DeleteOne(x => x.Account.Id == this.CurrentAccount().Id);
        }

        protected override IndustryNode FindRootNode() {
            return this.Collection.FindOne(x => x.Account.Id == this.CurrentAccount().Id);
        }

        protected override List<IndustryNode> GetChildNodes(IndustryNode node) {
            return node?.Children;
        }

        protected override IndustryNode GetUpdateNode(IndustryNode oldNode, IndustryNode newNode) {
            oldNode.Name = newNode.Name;
            return oldNode;
        }

        protected override void ReplaceRootNode(IndustryNode root) {
            this.Collection.UpdateOne(x => x.Account.Id == this.CurrentAccount().Id, builder => builder.Set(x => x.Name, root.Name).Set(x => x.Children, root.Children));
        }
    }
}
